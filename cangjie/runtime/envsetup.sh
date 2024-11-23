# Copyright Huawei Technologies Co., Ltd. 2020-2022. All rights reserved.
# This script needs to be placed in the output directory of Cangjie compiler.
# ** NOTE: Please use `source' command to execute this script. **

# Get current shell name.
shell_path=$(readlink -f /proc/$$/exe)
shell_name=${shell_path##*/}

# Get the absolute path of this script according to different shells.
case "${shell_name}" in
    "zsh")
        script_dir=$(cd "$(dirname "${(%):-%N}")"; pwd)
        ;;
    "sh" | "bash")
        script_dir=$(cd "$(dirname "${BASH_SOURCE[0]}")"; pwd)
        ;;
    *)
        echo "[ERROR] Unsupported shell: ${shell_name}, please switch to bash, sh or zsh."
        return 1
        ;;
esac

export PATH=${script_dir}/lib/windows_x86_64_llvm:${PATH}