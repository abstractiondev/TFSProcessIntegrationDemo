echo %1

call git submodule add git://github.com/abstractiondev/%1 Abstractions/%1
call git submodule update --init
