[CmdletBinding()]
param(
    [Parameter()]
    [ValidatePattern('^https://')]
    [string]$GatewayBaseUrl = 'https://api.ecosystemfx.net',

    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string[]]$HostName,

    [Parameter()]
    [hashtable]$ExpectedBrandByHost = @{}
)

$ErrorActionPreference = 'Stop'
$approvedFields = @(
    'backgroundColor',
    'brandId',
    'clientId',
    'clientUrl',
    'companyName',
    'documentType',
    'logoUrl',
    'name',
    'primaryColor',
    'secondaryColor',
    'supportEmail',
    'supportPhone'
) | Sort-Object

function Normalize-BrandHost {
    param([Parameter(Mandatory)][string]$Value)

    $candidate = $Value.Trim()
    if ($candidate -notmatch '://') {
        $candidate = "https://$candidate"
    }

    $uri = [Uri]$candidate
    $hostValue = $uri.IdnHost.TrimEnd('.').ToLowerInvariant()
    if ($hostValue.StartsWith('www.')) {
        return $hostValue.Substring(4)
    }
    return $hostValue
}

$gateway = $GatewayBaseUrl.TrimEnd('/')
foreach ($requestedHost in $HostName) {
    $normalizedHost = Normalize-BrandHost -Value $requestedHost
    $encodedHost = [Uri]::EscapeDataString($requestedHost)
    $uri = "$gateway/api/v1/brandconfiguration/public/current?host=$encodedHost"

    Write-Host "Verifying public branding for $normalizedHost"
    $response = Invoke-WebRequest -Uri $uri -Method Get -TimeoutSec 15
    if ($response.StatusCode -ne 200) {
        throw "Unexpected HTTP status $($response.StatusCode) for $normalizedHost"
    }

    $contract = [string]$response.Headers['X-Branding-Contract']
    if ($contract -ne 'public-branding-v1') {
        throw "Unexpected branding contract '$contract' for $normalizedHost"
    }

    $payload = $response.Content | ConvertFrom-Json
    if ($payload.success -ne $true -or $null -eq $payload.data) {
        throw "Invalid branding response envelope for $normalizedHost"
    }

    $data = $payload.data
    $actualFields = @($data.PSObject.Properties.Name | Sort-Object)
    if (Compare-Object -ReferenceObject $approvedFields -DifferenceObject $actualFields) {
        throw "Public branding fields changed for $normalizedHost"
    }

    if (
        [long]$data.brandId -le 0 -or
        [string]::IsNullOrWhiteSpace([string]$data.clientId) -or
        [string]::IsNullOrWhiteSpace([string]$data.name) -or
        [string]::IsNullOrWhiteSpace([string]$data.clientUrl) -or
        [string]::IsNullOrWhiteSpace([string]$data.supportEmail)
    ) {
        throw "Required branding data is incomplete for $normalizedHost"
    }

    $configuredHost = Normalize-BrandHost -Value ([string]$data.clientUrl)
    if ($configuredHost -ne $normalizedHost) {
        throw "ClientUrl resolves to '$configuredHost' instead of '$normalizedHost'"
    }

    if ($ExpectedBrandByHost.ContainsKey($normalizedHost)) {
        $expectedBrandId = [long]$ExpectedBrandByHost[$normalizedHost]
        if ([long]$data.brandId -ne $expectedBrandId) {
            throw "Expected BrandId $expectedBrandId but received $($data.brandId) for $normalizedHost"
        }
    }

    Write-Host "OK: $normalizedHost -> BrandId $($data.brandId), $($data.name)"
}
