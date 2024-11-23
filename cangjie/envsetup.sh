# Copyright Huawei Technologies Co., Ltd. 2020-2022. All rights reserved.
# This script needs to be placed in the output directory of Cangjie compiler.
# ** NOTE: Please use `source' command to execute this script. **

# Get current shell name.
shell_path=$(readlink -f /proc/$$/exe)
shell_name=${shell_path##*/}

# Get the absolute path of this script according to different shells.
case "${shell_name}" in
    "zsh")
        # check whether compinit has been executed 
        if (( ${+_comps} )); then
            # if compinit already executed, delete completion functions of cjc, cjc-frontend firstly
            compdef -d cjc cjc-frontend
        else
            autoload -Uz compinit
            compinit
        fi

        # auto complete cjc, cjc-frontend
        compdef _gnu_generic cjc cjc-frontend
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

export CANGJIE_HOME=${script_dir}

export PATH=${CANGJIE_HOME}/bin:${CANGJIE_HOME}/tools/bin:${CANGJIE_HOME}/tools/lib:${CANGJIE_HOME}/runtime/lib/windows_x86_64_llvm:${CANGJIE_HOME}/third_party/llvm/lldb/lib:$PATH:${USERPROFILE}/.cjpm/bin
