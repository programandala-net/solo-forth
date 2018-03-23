  \ tool.dump.system_variables.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803231401
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tools to dump the contents of system variables.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( .os-strms )

need os-chans need os-strms

: chan> ( n -- a ) 1- os-chans @ + ;
  \ Convert channel offset _n_ in `os-chans`, fetched from an
  \ element of `os-strms`, to its address _a_.

: chan>id ( n -- c ) chan> 4 + c@ ;
  \ Convert channel offset _n_ in `os-chans`, fetched from an
  \ element of `os-strms`, to its character identifier _c_.

: strm#> ( n -- a ) 3 + cells os-strms + ;
  \ Convert stream number _n_ to address _a_ of its
  \ corresponding element in `os-strms`.

: .os-strms ( -- )
  16 -3 ?do
    '#' emit i .  i strm#> @ ?dup
    if   dup u.
         dup chan>id ." -- channel '" emit ." ' at " chan> u.
    else ." Not attached"
    then cr
  loop ;

  \ doc{
  \
  \ .os-strms ( -- ) "dot-o-s-streams"
  \
  \ Display the contents of `os-strms`.
  \
  \ See: `.os-chans`.
  \
  \ }doc

( .os-chans )

need os-chans need char+

: .os-chans ( -- )
  os-chans @
  begin  dup c@ 128 <>
  while                dup u.
         ." Out:"      dup @ u.
         ." In:" cell+ dup @ u.
         ." Id:" cell+ dup c@ emit cr char+
  repeat drop ;

  \ doc{
  \
  \ .os-chans ( -- ) "dot-o-s-chans"
  \
  \ Display the contents of `os-chans`.
  \
  \ See: `.os-strms`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2018-03-23: Start. Add `.os-strms` and `.os-chans`.

  \ vim: filetype=soloforth
