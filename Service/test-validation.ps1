$body = @{
    requestCode = "550e8400-e29b-41d4-a716-446655440000"
    userName = ""
    jobNumber = 0
    cycleNumber = 1
    activityCode = ""
    workstation = ""
    applicationName = "TestApp"
    utcCreatedAt = "2025-08-15T10:30:00Z"
} | ConvertTo-Json

Write-Host "Testing validation errors with empty required fields..."
Write-Host "Request Body: $body"
Write-Host ""

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5277/api/access-requests" -Method POST -Body $body -ContentType "application/json" -ErrorAction Stop
    Write-Host "Response Status: $($response.StatusCode)"
    Write-Host "Response Body: $($response.Content)"
} catch {
    Write-Host "Error Status Code: $($_.Exception.Response.StatusCode.value__)"
    Write-Host "Error Status Description: $($_.Exception.Response.StatusDescription)"
    
    if ($_.Exception.Response.GetResponseStream()) {
        $errorResponse = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($errorResponse)
        $errorBody = $reader.ReadToEnd()
        Write-Host "Error Response Body: $errorBody"
        $reader.Close()
    }
}
