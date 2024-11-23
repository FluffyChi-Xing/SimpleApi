# Copyright Huawei Technologies Co., Ltd. 2022-2022. All rights reserved.
# This script needs to be placed in the root directory of installation of Cangjie compiler and libraries.

$script_dir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Windows searches for both binaries and libs in %Path%
$env:Path = $script_dir + "\lib\windows_x86_64_llvm;" + $env:Path
