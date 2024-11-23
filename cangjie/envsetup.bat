@REM Copyright Huawei Technologies Co., Ltd. 2022-2022. All rights reserved.
@REM This script needs to be placed in the root directory of installation of Cangjie compiler and libraries.

@echo off
REM Set CANGJIE_HOME to the path of this batch script.
set "CANGJIE_HOME=%~dp0"

REM Windows searches for both binaries and libs in %Path%
set "PATH=%CANGJIE_HOME%runtime\lib\windows_x86_64_llvm;%CANGJIE_HOME%bin;%CANGJIE_HOME%tools\bin;%CANGJIE_HOME%tools\lib;%PATH%;%USERPROFILE%\.cjpm\bin"
