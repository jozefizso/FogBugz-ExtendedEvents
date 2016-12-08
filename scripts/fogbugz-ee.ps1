function Post-Commit() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sCommitRevision
    )

    Post-ExtendedEvent -sEventType "commit" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sCommitRevision $sCommitRevision
}

function Post-BuildSuccess() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sBuildName
    )

    Post-ExtendedEvent -sEventType "build-success" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sBuildName $sBuildName
}


function Post-BuildFailure() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl,
        $sBuildName
    )

    Post-ExtendedEvent -sEventType "build-failure" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl `
                       -sBuildName $sBuildName
}


function Post-ReleaseNote() {
    param (
        $ixBug,
        $dtEventUtc,
        $sPersonName,
        $sMessage,
        $sExternalUrl
    )

    Post-ExtendedEvent -sEventType "releasenote" `
                       -ixBug $ixBug `
                       -dtEventUtc $dtEventUtc `
                       -sPersonName $sPersonName `
                       -sMessage $sMessage `
                       -sExternalUrl $sExternalUrl
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
        $sBuildName
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
