\ version_number.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ Last modified 202010012056
\ See change log at the end of the file

\ ==============================================================
\ Description

\ This program extracts the version number of Solo Forth from the
\ labels defined in the Z80 source and prints it in certain format.
\

\ This program is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth/).

\ ==============================================================
\ Usage

\ Invocation:

\     gforth -e 's" PATH-TO/version.z80s" flag' version_number.fs

\ Where "flag" is either `true` (complete version number) or `false`
\ (simplified format used as a file name for the background images).

\ ==============================================================
\ Author

\ Marcos Cruz (programandala.net), 2016, 2017.

\ ==============================================================
\ License

\ You may do whatever you want with this work, so long as you
\ retain every copyright, credit and authorship notice, and this
\ license.  There is no warranty.

\ ==============================================================

only forth definitions decimal

\ ==============================================================

variable real-format \ flag

variable version-major
variable version-minor
variable version-patch
variable version-prerelease-id
variable version-prerelease
variable version-build-high
variable version-build-low

: .version-part ( n -- )  s>d <# #S #> type ;
  \ Print one part of the version number.

: build-date ( -- u )
  version-build-low @ version-build-high @ $10000 * + ;

: .real-version ( -- )
  version-major @ .version-part '.' emit
  version-minor @ .version-part '.' emit
  version-patch @ .version-part
  version-prerelease @ ?dup if
    version-prerelease-id @ case
      'd' of ." -dev." .version-part endof
      'p' of ." -pre." .version-part endof
      'r' of ." -rc."  endof
    endcase .version-part
  then '+' emit build-date 0 .r ;

: .version-part-00 ( n -- )  s>d <# # # #> type ;
  \ Print one part of the version number, with two digits,
  \ padded with zeros.

: .simple-version ( -- )
  version-major @ .version-part-00 '.' emit
  version-minor @ .version-part-00 '.' emit
  version-patch @ .version-part-00 ;
  \ Print the version number using format "MM.mm.pp".

: .version ( -- )
  real-format @ if .real-version else .simple-version then ;

\ ==============================================================
\ Parser

: symbol ( a "name" )
  create ,
  does> ( "name1" "name2" -- ) ( pfa )
  parse-name 2drop parse-name evaluate swap @ ! ;
  \ Define a Z80 symbol _name_ to update the contents of variable _a_.
  \ When executed, _name_ will parse _name1_ (the Z80 "equ" directive)
  \ and _name2_ (the numeric value), and will store this value in the
  \ variable.

wordlist dup constant parser-wordlist set-current

version-major         symbol version_major:           ( "name1" name2" -- )
version-minor         symbol version_minor:           ( "name1" name2" -- )
version-patch         symbol version_patch:           ( "name1" name2" -- )
version-prerelease-id symbol version_prerelease_id:   ( "name1" name2" -- )
version-prerelease    symbol version_prerelease:      ( "name1" name2" -- )
version-build-high    symbol version_build_high_part: ( "name1" name2" -- )
version-build-low     symbol version_build_low_part:  ( "name1" name2" -- )

' \ alias ; ( "ccc<eol>" -- )  immediate
  \ Ignore the comments in the Z80 source file.

forth-wordlist set-current

\ ==============================================================
\ Main

real-format !  parser-wordlist >order included  .version bye

\ ==============================================================
\ Change log

\ 2016-11-19: Start.
\
\ 2016-12-30: Improve the description.
\
\ 2017-02-18: Rename from `versionfile2string.fs` to
\ `version_number.fs`. Improve to return also the real-format version
\ number, with prerelease and build date. Improve the parsing words
\ with a defining word.
\
\ 2017-07-26: Update to the new internal format of the version number.
\
\ 2020-10-01: Update to support dev/pre/rc prereleases.

