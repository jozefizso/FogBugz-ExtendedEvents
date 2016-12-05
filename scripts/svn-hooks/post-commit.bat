@echo off
rem POST-COMMIT HOOK
rem
rem The post-commit hook is invoked after a commit.  Subversion runs
rem this hook by invoking a program (script, executable, binary, etc.)
rem named 'post-commit' (for which this file is a template) with the 
rem following ordered arguments:
rem
rem   [1] REPOS-PATH   (the path to this repository)
rem   [2] REV          (the number of the revision just committed)
rem   [3] TXN-NAME     (the name of the transaction that has become REV)
rem
rem Because the commit has already completed and cannot be undone,
rem the exit code of the hook program is ignored.  The hook program
rem can use the 'svnlook' utility to help it examine the
rem newly-committed tree.

set PWSH=%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe
%PWSH% -NoProfile -ExecutionPolicy Bypass -command "$input | %1\hooks\post-commit.ps1" "%1" "%2" "%3"
if errorlevel 1 exit /b %errorlevel%
