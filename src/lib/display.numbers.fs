  \ display.numbers.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to number printing.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ud.r u.r ud. holds .00 .0000 )

unneeding ud.r
?\ : ud.r ( ud n -- ) >r <# #s #> r> over - 0 max spaces type ;

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ ud.r ( ud n -- ) "u-d-dot-r"
  \
  \ Display _ud_ right aligned in a field _n_ characters wide.
  \ If the number of characters required to display _ud_ is
  \ greater than _n_, all digits are displayed with no leading
  \ spaces in a field as wide as necessary.
  \
  \ See also: `u.r`, `d.`, `ud.`.
  \
  \ }doc

unneeding u.r ?( need u>ud need ud.r
: u.r ( u n -- ) >r u>ud r> ud.r ; ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ u.r ( u n -- ) "u-dot-r"
  \
  \ Display _u_ right aligned in a field _n_ characters wide.
  \ If the number of characters required to display _u_ is
  \ greater than _n_, all digits are displayed with no leading
  \ spaces in a field as wide as necessary.
  \
  \ Origin: Forth-79 (Reference Word Set)footnote:[In Forth-79,
  \ if the number of characters required to display _u_ is
  \ greater than _n_, no leading spaces are given.], Forth-83
  \ (Controlled Reference Word Set)footnote:[In Forth-83, if
  \ the number of characters required to display _u_ is greater
  \ than _n_, an error condition exists, which depends on the
  \ system.], Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `ud.r`, `.r`, `u.`.
  \
  \ }doc

unneeding ud. ?( need ud.r
: ud. ( ud -- ) 0 ud.r space ; ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ ud. ( ud -- ) "u-d-dot"
  \
  \ Display an usigned double number _ud_.
  \
  \ See also: `ud.r`, `d.`, `u.`.
  \
  \ }doc

unneeding holds ?(

: holds ( ca len -- )
  begin  dup  while  1- 2dup + c@ hold  repeat 2drop ; ?)

  \ Credit:
  \ Code from the documentation of Forth-2012.

  \ doc{
  \
  \ holds ( ca len -- )
  \
  \ Add string _ca len_ to the pictured numeric output string
  \ started by `<#`.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ See also: `hold`.
  \
  \ }doc

unneeding .00 ?\ : .00 ( +n -- ) s>d <# # # #> type ;

  \ doc{
  \
  \ .00 ( +n -- ) "dot-zero-zero"
  \
  \ Display _+n_ with two digits.
  \
  \ See also: `.0000`, `.time`, `.date`.
  \
  \ }doc

unneeding .0000 ?\ : .0000 ( +n -- ) s>d <# # # # # #> type ;

  \ doc{
  \
  \ .0000 ( +n -- ) "dot-zero-zero-zero-zero"
  \
  \ Display _+n_ with four digits.
  \
  \ See also: `.00`, `.date`.
  \
  \ }doc

( base. bin. hex. )

  \ Credit:
  \
  \ Code modified from eForth.

unneeding base.
?\ : base. ( -- ) does> c@ base @ >r base ! u. r> base ! ;

unneeding bin.
?\ need base.  create bin. ( n -- ) #2 c, base.

  \ doc{
  \
  \ bin. ( n -- ) "bin-dot"
  \
  \ Display _n_ as an unsigned binary number, followed by
  \ one space.
  \
  \ See also: `dec.`, `hex.`, `u.`, `.`.
  \
  \ }doc

unneeding hex.
?\ need base.  create hex. ( n -- ) #16 c, base.

  \ doc{
  \
  \ hex. ( n -- ) "hex-dot"
  \
  \ Display _n_ as an unsigned hexadecimal number, followed by
  \ one space.
  \
  \ See also: `dec.`, `bin.`, `u.`, `.`.
  \
  \ }doc

  \ unneeding dec.
  \ ?\ need base.  create dec. ( n -- ) #10 c, base.
  \ XXX OLD -- `dec.` is in the kernel

( base' (d. <hex hex> (dhex. 8hex. 16hex. 32hex. )

  \ Credit:
  \
  \ Code partly adapted from lina.

unneeding base' unneeding base> and ?(

variable base'  : base> ( -- ) base' @ base ! ; ?)

  \ doc{
  \
  \ base' ( -- a ) "base-tick"
  \
  \ A temporary variable used by `<hex`, `hex>`, `<bin` and
  \ `bin>`.  to store the current value of `base`.
  \
  \ See also: `abase`.
  \
  \ }doc

  \ doc{
  \
  \ base> ( -- ) "base-from"
  \
  \ Restore the previous value of `base` from `base'`.
  \ ``base>`` is executed by `bin>` and `hex>`.
  \
  \ }doc

unneeding (d.
?\ : (d. ( d n -- ca len ) <# 0 ?do  #  loop  #> ;

  \ doc{
  \
  \ (d. ( d n -- ca len ) "paren-d-dot"
  \
  \ Convert _d_ to an unsigned number in the current `base`,
  \ with _n_ digits, as string _ca len_.
  \
  \ See also: `(dbin.`, `(dhex.`.
  \
  \ }doc

unneeding <hex unneeding hex> and ?( need base' need base>
: <hex ( -- ) base @ base' ! hex ; : hex> ( -- ) base> ; ?)

  \ doc{
  \
  \ <hex ( -- ) "start-hex"
  \
  \ Start a code zone where hexadecimal radix is the default,
  \ by save the current value of `base` to `base'` and
  \ executing `hex`. The zone is finished by `hex>`.
  \
  \ Origin: lina.
  \
  \ See also: `<bin`.
  \
  \ }doc

  \ doc{
  \
  \ hex> ( -- ) "end-hex"
  \
  \ End a code zone where hexadecimal radix is the default, by
  \ restoring the value of `base` from `base'`.  The zone was
  \ started by `<hex`.
  \
  \ }doc

unneeding (dhex. dup ?\ need <hex need (d.
?\ : (dhex. ( d n -- ) <hex (d. hex> type space ;

  \ doc{
  \
  \ (dhex. ( d n -- ) "paren-d-hex-dot"
  \
  \ Display _d_ as an unsigned hexadecimal number with _n_ digits.
  \
  \ See also: `(dbin.`, `32hex.`, `16hex.`, `8hex.`, `hex.`.
  \
  \ }doc

unneeding 32hex.
?\ need (dhex.  : 32hex. ( d -- ) 8 (dhex. ;

  \ doc{
  \
  \ 32hex. ( d -- ) "32-hex-dot"
  \
  \ Display _d_ as an unsigned 32-bit hexadecimal number.
  \
  \ See also: `32bin.`, `16hex.`, `8hex.`, `hex.`, `hex`.
  \
  \ }doc

unneeding 16hex.
?\ need (dhex.  : 16hex. ( n -- ) s>d 4 (dhex. ;

  \ doc{
  \
  \ 16hex. ( d -- ) "16-hex-dot"
  \
  \ Display _d_ as an unsigned 16-bit hexadecimal number.
  \
  \ See also: `16bin.`, `32hex.`, `8hex.`, `hex.`, `hex`.
  \
  \ }doc

unneeding 8hex.
?\ need (dhex.  : 8hex. ( b -- ) s>d 2 (dhex. ;

  \ doc{
  \
  \ 8hex. ( d -- ) "8-hex-dot"
  \
  \ Display _d_ as an unsigned 8-bit hexadecimal number.
  \
  \ See also: `8bin.`, `16hex.`, `hex.`, `hex`.
  \
  \ }doc

( binary <bin bin> (dbin. 8bin. 16bin. 32bin. )

  \ Credit:
  \
  \ Code inspired by lina.

unneeding binary ?\ : binary ( -- ) 2 base ! ;

  \ doc{
  \
  \ binary ( -- )
  \
  \ Set contents of `base` to two.
  \
  \ See also: `decimal`, `hex`.
  \
  \ }doc

unneeding <bin unneeding bin> and ?(
need base' need base> need binary
: <bin ( -- ) base @ base' ! binary ; : bin> ( -- ) base> ; ?)

  \ doc{
  \
  \ <bin ( -- ) "start-bin"
  \
  \ Start a code zone where binary radix is the default, by
  \ saving the current value of `base` to `base'` and executing
  \ `binary`. The zone is finished by `bin>`.
  \
  \ See also: `<hex`.
  \
  \ }doc

  \ doc{
  \
  \ bin> ( -- ) "end-bin"
  \
  \ End a code zone where binary radix is the default, by
  \ restoring the value of `base` from `base'`.  The zone was
  \ started by `<bin`.
  \
  \ }doc

unneeding (dbin. dup
?\ need <bin need (d.
?\ : (dbin. ( d n -- ) <bin (d. bin> type space ;

  \ doc{
  \
  \ (dbin. ( d n -- ) "paren-d-bin-dot"
  \
  \ Display _d_ as an unsigned binary number with _n_ digits.
  \
  \ See also: `(dhex.`, `32bin.`, `16bin.`, `8bin.`, `bin.`.
  \
  \ }doc

unneeding 32bin.
?\ need (dbin.  : 32bin. ( d -- ) #32 (dbin. ;

  \ doc{
  \
  \ 32bin. ( d -- ) "32-bin-dot"
  \
  \ Display _d_ as an unsigned 32-bit binary number.
  \
  \ See also: `32hex.`, `16bin.`, `8bin.`, `bin.`, `binary`.
  \
  \ }doc

unneeding 16bin.
?\ need (dbin.  : 16bin. ( n -- ) s>d #16 (dbin. ;

  \ doc{
  \
  \ 16bin. ( n -- ) "16-bin-dot"
  \
  \ Display _n_ as an unsigned 16-bit binary number.
  \
  \ See also: `16bin.`, `32bin.`, `8bin.`, `bin.`, `binary`.
  \
  \ }doc

unneeding 8bin.
?\ need (dbin.  : 8bin. ( b -- ) s>d  #8 (dbin. ;

  \ doc{
  \
  \ 8bin. ( n -- ) "8-bin-dot"
  \
  \ Display _n_ as an unsigned 8-bit binary number.
  \
  \ See also: `8hex.`, `32bin.`, `16bin.`, `bin.`, `binary`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-01: Add `holds`.
  \
  \ 2016-11-24: Move `u.r` from the kernel and document it.
  \ Document also `ud.r` and `ud.`. Improve individual needing
  \ of all of them.
  \
  \ 2016-12-08: Fix `u.r`. Fix needing of `ud.`.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation. Rename `(d.)` to `(d.`,
  \ `(dbin.)` to `(dbin.` and `(dhex.)` to `(dhex.`.  Make all
  \ words individually accessible to `need`.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-25: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-09: Fix typo.
  \
  \ 2017-12-10: Move `.00` and `.0000` from <time.fs>.
  \ Document them.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2020-05-04: Fix cross references: `(bin.` -> `(dbin.`,
  \ `(hex.` -> `(dhex.`.

  \ vim: filetype=soloforth
