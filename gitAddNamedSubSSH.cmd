echo %1 as %2

call git submodule add git@github.com:abstractiondev/%1 Abstractions/%2
call git submodule update --init
