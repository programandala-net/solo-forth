  \ string.cquote.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702272344
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Two implementations of `c"` and
  \ `csliteral`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( cslit csliteral c" )

  \ This is the default definition of `csliteral`, based on a
  \ system-dependent `cslit`, which makes it possible to decode
  \ `c"`.

  \ Data space used: 43 bytes.

: cslit ( -- ca ) r@ dup c@ 1+ r> + >r ;
  \ doc{
  \
  \ cslit ( -- ca )
  \
  \ Return a string that is compiled after the calling word, and
  \ adjust the instruction pointer to step over the inline string.
  \
  \ ``cslit`` is compiled by `csliteral`.
  \
  \ See also: `slit`.
  \
  \ }doc

: csliteral ( ca len -- )
  compile cslit s, ; immediate compile-only
  \ doc{
  \
  \ csliteral ( Compilation: "ccc<quote>" -- ) ( Run-time: -- ca )
  \
  \ Compile a string _ca len_ which at run-time will
  \ be returned as a counted string.
  \
  \ ``csliteral`` is an `immediate` and `compile-only` word.
  \
  \ See also: `sliteral`, `cslit`.
  \
  \ }doc

: c" ( Compilation: "ccc<quote>" -- )
      ( Run-time: -- ca )
  '"' parse postpone csliteral ; immediate compile-only
  \ doc{
  \
  \ c" ( Compilation: "ccc<quote>" -- ) ( Run-time: -- ca )
  \
  \ Parse a string delimited by double quotes and
  \ compile it into the current definition.
  \ At run-time the string will be returned as a
  \ counted string _ca_.
  \
  \ ``c"`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `csliteral`.
  \
  \ }doc

exit

  \ This is an alternative system-independent definition of
  \ `csliteral`.

: csliteral ( Compilation: ca len -- )
             ( Run-time: -- ca )
  2>r postpone ahead here 2r> s, >r postpone then
  r> postpone literal ; immediate compile-only
  \ Credit:
  \ Code from Gforth's `CLiteral`.

  \ ===========================================================
  \ Change log

  \ 2016-03-14: Write `c"` with a new, system-independent
  \ implementation of `csliteral`.
  \
  \ 2016-03-15: Set the previous, system-dependent
  \ implementation of `csliteral` as default.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-08-05: Combine both blocks. Keep the alternative
  \ definition of `csliteral` only as a reference.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
