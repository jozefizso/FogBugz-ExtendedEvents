function Post-Commit() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sCommitRevision,
        $sModuleName,
        $sBranchName
    )

    Post-ExtendedEvent -sEventType "commit" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sCommitRevision $sCommitRevision `
                       -sModuleName $sModuleName `
                       -sBranchName $sBranchName
}

function Post-BuildSuccess() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sBuildName,
        $sModuleName,
        $sBranchName
    )

    Post-ExtendedEvent -sEventType "build-success" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sBuildName $sBuildName `
                       -sModuleName $sModuleName `
                       -sBranchName $sBranchName
}


function Post-BuildFailure() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sBuildName,
        $sModuleName,
        $sBranchName
    )

    Post-ExtendedEvent -sEventType "build-failure" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sBuildName $sBuildName `
                       -sModuleName $sModuleName `
                       -sBranchName $sBranchName
}


function Post-ReleaseNote() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sModuleName,
        $sBranchName
    )

    Post-ExtendedEvent -sEventType "releasenote" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sModuleName $sModuleName `
                       -sBranchName $sBranchName
}


function Post-ExtendedEvent() {
    param (
        $ixBug,
        $sEventType,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sCommitRevision,
        $sBuildName,
        $sModuleName,
        $sBranchName
    )

    $sPluginId="FBExtendedEvents@goit.io"

    $params = @{
      "sAction" = "event"
      "ixBug"= $ixBug
      "sEventType" = $sEventType
      "dtEventUtc"= $dtEventUtc
      "sPersonName" = $sPersonName
      "sMessage"= $sMessage
      "sExternalUrl" = $sExternalUrl
      "sCommitRevision"= $sCommitRevision
      "sBuildName" = $sBuildName
      "sModuleName" = $sModuleName
      "sBranchName" = $sBranchName
      "token" = $sToken
    }

    $url = "$cFbUrl/default.asp?pg=pgPluginRaw&sPluginId=$sPluginId"
    try {
        $tmp = Invoke-WebRequest -Uri $url -Method POST -Body $params -TimeoutSec 30 -ErrorAction SilentlyContinue

        Write-Host "Created extended event. $tmp"
    }
    catch [Exception] {
        Write-Error "Failed to post data to URL: $url ($_)"
    }
}
