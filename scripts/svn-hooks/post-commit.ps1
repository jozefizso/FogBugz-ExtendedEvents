param(
  $repo,
  $rev,
  $txnName
)

$base_path = Split-Path $MyInvocation.MyCommand.Path

& "$base_path\post-commit-fogbugz.ps1" $repo $rev $txnName
& "$base_path\post-commit-fogbugz-ee.ps1" $repo $rev $txnName
