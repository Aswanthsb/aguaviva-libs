@echo off
echo M80/L80 Z80 Compiler - IBM PC
echo Ported by A&L Software
echo ÿ
if "%1"=="" goto error
if not exist %1.mac goto error
echo MSX.M-80  1.00  01-Apr-85  (c) 1981,1985 Microsoft
m80 =%1.mac/r/z%2%3%4%5%6%7%8
_clerror
if not errorlevel 1 goto end
l80 %1,%1/e/n
if exist %1.com del %1.com > nul
ren %1.cpm *.com > nul
del %1.rel > nul
goto end
:error
echo Usage:
echo CL80 program [options]
echo ÿ
echo [options] are any of M80 options, except /L and /Z
echo program   is the source code (.MAC) to compiled, without extension
:end
