# Copyright Huawei Technologies Co., Ltd. 2022-2022. All rights reserved.
# This script needs to be placed in the root directory of installation of Cangjie compiler and libraries.

# Set CANGJIE_HOME to the path of this batch script.
$env:CANGJIE_HOME = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Windows searches for both binaries and libs in %Path%
$env:Path = $env:CANGJIE_HOME + "\runtime\lib\windows_x86_64_llvm;" + $env:Path
$env:Path = $env:CANGJIE_HOME + "\lib\windows_x86_64_llvm;" + $env:Path
$env:Path = $env:CANGJIE_HOME + "\bin;" + $env:Path 
$env:Path = $env:CANGJIE_HOME + "\tools\bin;" + $env:Path
$env:Path = $env:Path + ";" + $env:USERPROFILE + "\.cjpm\bin"
$env:Path = $env:CANGJIE_HOME + "\tools\lib;" + $env:Path
