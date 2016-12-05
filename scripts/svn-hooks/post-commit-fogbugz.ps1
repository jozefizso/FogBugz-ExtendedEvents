param(
  $repo,
  $rev,
  $txnName
)

$cFbUrl="http://localhost/fb"
$cIxRepo="1"

$sNew=[int]$rev
$sPrev=$sNew-1

$changes = (& svnlook.exe changed -r $rev $repo)
$message = (& svnlook.exe log -r $rev $repo)

$files = $changes | where { $_ -match "\s*(.)\s+(.+)" } | foreach { $matches[2] }
$bugzIds = [regex]::matches($message, '\s*BugzID\s*[: ]+(\d+)') | % { $_.Groups[1].Value }

$bugzIds  | foreach {
    $ixBug = [uri]::EscapeDataString($_)
    $files | foreach {
        $file = $_
        
        $sFile = [uri]::EscapeDataString($file)
        $url = "$cFbUrl/cvsSubmit.asp?ixBug=$ixBug&sFile=$sFile&sNew=$sNew&sPrev=$sPrev&ixRepository=$cIxRepo"
        
        try {
            $tmp = Invoke-WebRequest -Uri $url -Method GET -TimeoutSec 30 -ErrorAction SilentlyContinue
        }
        catch [Exception] {
            Write-Error "Failed to post data to URL: $url ($_)"
        }
    }
}
