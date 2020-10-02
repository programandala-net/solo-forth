\ version_number.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ Last modified 202010021653
\ See change log at the end of the file

\ ==============================================================
\ Description

\ This program extracts the version number of Solo Forth from the
\ labels defined in the Z80 source and prints it in certain format.

\ This program is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth/).

\ ==============================================================
\ Usage

\ Invocation:

\     gforth -e 's" PATH-TO/version.z80s" FORMAT' version_number.fs

\ Where FORMAT is 0..3 with the following meaning:
\
\ 0 = XX.YY.ZZ (two digits per element, with padding zeros)
\ 1 = X.Y.Z
\ 2 = X.Y.Z-prerelease
\ 3 = X.Y.Z-prerelease+build

\ ==============================================================
\ Author

\ Marcos Cruz (programandala.net), 2016, 2017, 2020.

\ ==============================================================
\ License

\ You may do whatever you want with this work, so long as you
\ retain every copyright, credit and authorship notice, and this
\ license.  There is no warranty.

\ ==============================================================

only forth definitions decimal

\ ==============================================================

variable format \ version output format ID (0..2)

variable version-major
variable version-minor
variable version-patch
variable version-prerelease-id
variable version-prerelease
variable version-build-high
variable version-build-low

defer .version-part

: .full-version-part ( n -- )  s>d <# #S #> type ;
  \ Print one part of the version number.
  \ Default action of `.version-part`.

: .version-part-00 ( n -- )  s>d <# # # #> type ;
  \ Print one part of the version number, with two digits,
  \ padded with zeros.
  \ Alternative action of `.version-part`.

' .full-version-part is .version-part

: build-date ( -- u )
  version-build-low @ version-build-high @ $10000 * + ;

: .version-x.y.z ( -- )
  version-major @ .version-part '.' emit
  version-minor @ .version-part '.' emit
  version-patch @ .version-part ;
  \ Print the version number using format "X.Y.Z".

: .version-0x.0y.0z ( -- )
  ['] .version-part-00 is .version-part
  .version-x.y.z ;

: .version-x.y.z-prerelease ( -- )
  .version-x.y.z
  version-prerelease @ ?dup if
    version-prerelease-id @ case
      'd' of ." -dev." endof
      'p' of ." -pre." endof
      'r' of ." -rc."  endof
    endcase .version-part
  then ;

: .version-x.y.z-prerelease+build ( -- )
  .version-x.y.z-prerelease '+' emit build-date 0 .r ;

: .version ( -- )
  format @ case
    0 of .version-0x.0y.0z               endof
    1 of .version-x.y.z                  endof
    2 of .version-x.y.z-prerelease       endof
    3 of .version-x.y.z-prerelease+build endof
  endcase ;

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

format ! parser-wordlist >order included .version bye

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
\
\ 2020-10-02: Improve format selection from true/false to 0..3.

\ vim: filetype=gforth
