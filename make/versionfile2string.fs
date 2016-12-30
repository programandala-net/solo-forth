#! /usr/bin/env gforth

\ versionfile2string.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ Last modified 201612302013

\ ==============================================================
\ Description

\ This program extracts the version numbers (major, minor, patch) from
\ label defined in the Z80 source and prints the whole version number
\ as a formatted string, which is used in <Makefile> as part of the
\ filename of the current background image.

\ Usage:

\     versionfile2string.fs PATH-TO/version.z80s

\ This program is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth/).

\ ==============================================================
\ Author

\ Marcos Cruz (programandala.net), 2016.

\ ==============================================================
\ License

\ You may do whatever you want with this work, so long as you
\ retain every copyright, credit and authorship notice, and this
\ license.  There is no warranty.

\ ==============================================================
\ History

\ 2016-11-19: Start.
\
\ 2016-12-30: Improve the description.

\ ==============================================================

only forth definitions

variable version-major
variable version-minor
variable version-patch

: .version-part  ( n -- )  s>d <# # # #> type  ;
  \ Print one part of the version number, using two digits in order to
  \ make the final string sortable as part of a filename.

: .version  ( -- )
  version-major @ .version-part '.' emit
  version-minor @ .version-part '.' emit
  version-patch @ .version-part  ;
  \ Print the version number using format "MM.mm.pp".

wordlist constant parser-wordlist
parser-wordlist set-current

\ XXX TODO -- Write `fetcher` and `ignorer` definers.

' \ alias ;  ( "ccc<eol>" -- )  immediate

: version_major:  ( "name1" "name2" -- )
  parse-name 2drop parse-name evaluate version-major !  ;

: version_minor:  ( "name1" "name2" -- )
  parse-name 2drop parse-name evaluate version-minor !  ;

: version_patch:  ( "name1" "name2" -- )
  parse-name 2drop parse-name evaluate version-patch !  ;

' \ alias version_prerelease: ( "ccc<eol>" -- )
' \ alias version_build_high_part: ( "ccc<eol>" -- )
' \ alias version_build_low_part: ( "ccc<eol>" -- )

forth-wordlist set-current

parser-wordlist >order

s" ../" 1 arg s+ included
  \ Parse the file passed as first argument.

.version bye
