  \ tool.debug.where.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `where` tool, which displays information about the last
  \ error.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( where )

: where ( -- )
  error-pos 2@ ( n u )
  \ n = value of `>in` when the error happened
  \ u = value of `blk` when the error happened
  dup if
    ." Block #" dup dec. cr
    swap c/l /mod c/l * rot block + c/l type cr
    parsed-name @ - spaces '^' emit
  else 2drop then ;

  \ doc{
  \
  \ where ( -- )
  \
  \ Display information about the last error: block number, line
  \ number and a picture of where it occurred. If the error was
  \ in the command line, nothing is displayed.
  \
  \ Origin: Forth-79 (Reference Word Set).
  \
  \ See also: `error-pos`, `error`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-21: 3 bytes shorter.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-11-17: Remove unused `need [if]`.
  \
  \ 2017-01-18: The temporary version in the kernel has been
  \ removed. Improve documentation. Simplify: do not update
  \ `scr`.
  \
  \ 2017-01-26: Fix stack underflow. Change title to "Block"
  \ and print its number as unsigned (no practical difference
  \ when using floppy disks, but block numbers are unsigned).
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2018-09-24: Improve documentation. Simplify with `dec.`.
  \
  \ 2020-06-09: Fix the error position marker displayed by
  \ `where`: replace `here c@` (the value left by `word` in the
  \ old fig-Forth model) with `parsed-name @`.

  \ vim: filetype=soloforth
