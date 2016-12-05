# FogBugz Extended Events plugin script
param(
  $repo,
  $rev,
  $txnName
)

$cFbUrl="http://localhost/fb"
$cIxRepo="1"
$sPluginId="FBExtendedEvents@goit.io"
$sToken=""

$sRevision=$rev

$info = (& svnlook.exe info -r $rev $repo) -split "`n"

$sAuthor=$info[0]
$dtCommit = [datetime]::Parse($info[1].Substring(0, 25)).ToUniversalTime()
$sDateCommit =$dtCommit.ToString('o')

$message = (& svnlook.exe log -r $rev $repo)
$bugzIds = [regex]::matches($message, '\s*BugzID\s*[: ]+(\d+)') | % { $_.Groups[1].Value }
$sMessage = $message -join "`n"

$bugzIds | foreach {
    $ixBug = $_
    
    $params = @{
      "sAction" = "commit"
      "ixBug" = $ixBug
      "ixRepository" = $cIxRepo
      "sRevision" = $sRevision
      "dtCommit" = $sDateCommit
      "sAuthor" = $sAuthor
      "sMessage" = $sMessage
      "token" = $sToken
    }
    
    $url = "$cFbUrl/default.asp?pg=pgPluginRaw&sPluginId=$sPluginId"
    try {
        $tmp = Invoke-WebRequest -Uri $url -Method GET -Body $params -TimeoutSec 30 -ErrorAction SilentlyContinue
    }
    catch [Exception] {
        Write-Error "Failed to post data to URL: $url ($_)"
    }
}
