  \ flow.cases-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 20160324

  \ -----------------------------------------------------------
  \ Description

  \ `cases:` structure, an alternative to the standard `case`.
  \
  \ The `cases:` structure is named.  It's built as an array of
  \ pairs (value and associated vector).  It saves space, but
  \ is slower than standard `case`. The default case of the
  \ structure is mandatory.

  \ -----------------------------------------------------------
  \ Authors

  \ Original code written by Dan Lerner, published on Forth
  \ Dimensions (volume 3, number 6, page 189, 1982-03).

  \ Marcos Cruz (programandala.net), adapted, renamed and
  \ commented the original code, 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-11-14: First version.
  \ 2016-04-27: Rename `other>` to `othercase>`. Improve
  \ documentation and file header.

( cases: )

  \ 109 bytes used

: cases: ( "name" -- orig 0 )
  create >mark 0
  does> ( selector -- ) ( selector pfa )
    true rot rot dup ( true selector pfa pfa )
    cell+ swap @   ( true selector pfa+2 options )
    0 do ( true selector a )
      2dup @ = ( true selector a f )
      if    dup cell+ perform
            2>r 0= 2r> ( false selector a ) leave
      else  cell+ cell+  then
   loop ( true selector a | false selector a )
   rot if  perform  else  drop  then  drop ;

  \ doc{
  \
  \ cases: ( "name" -- orig 0 )
  \
  \ Define a `cases:` structure "name", built as an array of
  \ pairs (value and associated vector).
  \

  \ Usage example:
  \
  \ ----
  \ : say10      ." dek" ;
  \ : say100     ." cent" ;
  \ : say1000    ." mil" ;
  \ : say-other  ." alia" ;
  \
  \ cases: say ( n -- )
  \     10 case>      say10
  \    100 case>      say100
  \   1000 case>      say1000
  \        othercase> say-other
  \
  \ 10 say  100 say  1000 say  1001 say
  \ ----
  \
  \ }doc

: case> ( orig counter selector "name" -- orig counter' )
  , ' compile, 1+ ;

  \ doc{
  \
  \ case> ( orig counter selector "name" -- orig counter' )
  \
  \ Compile an option into a `cases:` structure. The given
  \ _selector_ will cause the word "name" to be executed.
  \
  \ See `cases:` for an usage example.
  \
  \ }doc

: othercase> ( orig counter "name" -- ) ' compile, swap ! ;

  \ doc{
  \
  \ othercase> ( orig counter "name" -- )
  \
  \ Compile the default option of a `cases:` to be the word
  \ "name" . This must be the last option of the structure and
  \ is mandatory.  When no default action is required,
  \ `othercase> noop` can be used.
  \
  \ See `cases:` for an usage example.
  \
  \ }doc

  \ vim: filetype=soloforth