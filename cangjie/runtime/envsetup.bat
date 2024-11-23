@REM Copyright Huawei Technologies Co., Ltd. 2020-2022. All rights reserved.
@REM This script needs to be placed in the root directory of installation of Cangjie compiler and libraries.

@echo off
REM Windows searches for both binaries and libs in %Path%
set "PATH=%~dp0lib\windows_x86_64_llvm;%PATH%"