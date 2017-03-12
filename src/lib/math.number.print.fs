  \ math.number.print.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702251949
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to number printing.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ud.r u.r ud. holds )

[unneeded] ud.r ?(
: ud.r ( ud n -- ) >r <# #s #> r> over - 0 max spaces type ;
?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ ud.r ( ud n -- )
  \
  \ Print an usigned double number _ud_ right justified in a
  \ field of _n_ characters wide. If the number of characters
  \ required to print _ud_ is greater than _n_, all digits are
  \ displayed with no leading spaces in a field as wide as
  \ necessary.
  \
  \ See also: `u.r`, `d.`, `ud.`.
  \
  \ }doc

[unneeded] u.r ?( need u>ud need ud.r
: u.r ( u n -- ) >r u>ud r> ud.r ; ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ u.r ( u n -- )
  \
  \ Print an unsigned number _u_ right justified in a field of
  \ _n_ characters wide. If the number of characters required
  \ to print _u_ is greater than _n_, all digits are displayed
  \ with no leading spaces in a field as wide as necessary.
  \
  \ Origin: Forth-79 (Reference Word Set)footnote:[In Forth-79,
  \ if the number of characters required to display _u_ is
  \ greater than _n_, no leading spaces are given.], Forth-83
  \ (Controlled Reference Word)footnote:[In Forth-83, if the
  \ number of characters required to display _u_ is greater
  \ than _n_, an error condition exists, which depends on the
  \ system.], Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `ud.r`, `.r`, `u.`.
  \
  \ }doc

[unneeded] ud. ?( need ud.r
: ud. ( ud -- ) 0 ud.r space ; ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ ud. ( ud -- )
  \
  \ Print an usigned double number _ud_.
  \
  \ See also: `ud.r`, `d.`, `u.`.
  \
  \ }doc

[unneeded] holds ?(

: holds ( ca len -- )
  begin  dup  while  1- 2dup + c@ hold  repeat 2drop ; ?)

  \ Credit:
  \ Code from the documentation of Forth-2012.

  \ doc{
  \
  \ holds ( ca len -- )
  \
  \ Add string _ca len_ to the pictured numeric output string.
  \
  \ Origin: Forth-2012 (CORE EXT).
  \
  \ }doc

( base. bin. hex. )

  \ Credit:
  \
  \ Code modified from eForth.

[unneeded] base.
?\ : base. ( -- ) does> c@ base @ >r base ! u. r> base ! ;

[unneeded] bin.
?\ need base.  create bin. ( n -- ) #2 c, base.

  \ doc{
  \
  \ bin. ( n -- )
  \
  \ Display _n_ as an unsigned binary number, followed by
  \ one space.
  \
  \ See also: `dec.`, `hex.`, `u.`, `.`.
  \
  \ }doc

[unneeded] hex.
?\ need base.  create hex. ( n -- ) #16 c, base.

  \ doc{
  \
  \ hex. ( n -- )
  \
  \ Display _n_ as an unsigned hexadecimal number, followed by
  \ one space.
  \
  \ See also: `dec.`, `bin.`, `u.`, `.`.
  \
  \ }doc

  \ [unneeded] dec.
  \ ?\ need base.  create dec. ( n -- ) #10 c, base.
  \ XXX OLD -- `dec.` is in the kernel

( base' (d. <hex hex> (dhex. 8hex. 16hex. 32hex. )

  \ Credit:
  \
  \ Code partly adapted from lina.

[unneeded] base' [unneeded] base> and ?(

variable base'  : base> ( -- ) base' @ base ! ; ?)

  \ doc{
  \
  \ base' ( -- a )
  \
  \ A temporary variable used by `<hex`, `hex>`, `<bin` and
  \ `bin>`.  to store the current value of `base`.
  \
  \ }doc

  \ doc{
  \
  \ base> ( -- )
  \
  \ Restore the previous value of `base` from `base'`. This is
  \ the word executed by `bin>` and `hex>`.
  \
  \ }doc

[unneeded] (d.
?\ : (d. ( d n -- ca len ) <# 0 ?do  #  loop  #> ;

  \ doc{
  \
  \ (d. ( d n -- ca len )
  \
  \ Convert _d_ to an unsigned number in the current `base`,
  \ with _n_ digits, as string _ca len_.
  \
  \ See also: `(bin.`, `(hex.`.
  \
  \ }doc

[unneeded] <hex [unneeded] hex> and ?( need base' need base>
: <hex ( -- ) base @ base' ! hex ; : hex> ( -- ) base> ; ?)

  \ doc{
  \
  \ <hex ( -- )
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
  \ hex> ( -- )
  \
  \ End a code zone where hexadecimal radix is the default, by
  \ restoring the value of `base` from `base'`.  The zone was
  \ started by `<hex`.
  \
  \ }doc

[unneeded] (dhex. dup ?\ need <hex need (d.
?\ : (dhex. ( d n -- ) <hex (d. hex> type space ;

  \ doc{
  \
  \ (dhex. ( d n -- )
  \
  \ Display _d_ as an unsigned hexadecimal number with _n_ digits.
  \
  \ See also: `(bin.`, `32hex.`, `16hex.`, `8hex.`, `hex.`.
  \
  \ }doc

[unneeded] 32hex.
?\ need (dhex.  : 32hex. ( d -- ) 8 (dhex. ;

  \ doc{
  \
  \ 32hex. ( d -- )
  \
  \ Display _d_ as an unsigned 32-bit hexadecimal number.
  \
  \ See also: `32bin.`, `16hex.`, `8hex.`, `hex.`, `hex`.
  \
  \ }doc

[unneeded] 16hex.
?\ need (dhex.  : 16hex. ( n -- ) s>d 4 (dhex. ;

  \ doc{
  \
  \ 16hex. ( d -- )
  \
  \ Display _d_ as an unsigned 16-bit hexadecimal number.
  \
  \ See also: `16bin.`, `32hex.`, `8hex.`, `hex.`, `hex`.
  \
  \ }doc

[unneeded] 8hex.
?\ need (dhex.  : 8hex. ( b -- ) s>d 2 (dhex. ;

  \ doc{
  \
  \ 8hex. ( d -- )
  \
  \ Display _d_ as an unsigned 8-bit hexadecimal number.
  \
  \ See also: `8bin.`, `16hex.`, `16hex.`, `hex.`, `hex`.
  \
  \ }doc

( binary <bin bin> (dbin. 8bin. 16bin. 32bin. )

  \ Credit:
  \
  \ Code inspired by lina.

[unneeded] binary ?\ : binary ( -- ) 2 base ! ;

  \ doc{
  \
  \ binary ( -- )
  \
  \ Set contents of `base` to two.
  \
  \ See also: `decimal`, `hex`.
  \
  \ }doc

[unneeded] <bin [unneeded] bin> and ?(
need base' need base> need binary
: <bin ( -- ) base @ base' ! binary ; : bin> ( -- ) base> ; ?)

  \ doc{
  \
  \ <bin ( -- )
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
  \ bin> ( -- )
  \
  \ End a code zone where binary radix is the default, by
  \ restoring the value of `base` from `base'`.  The zone was
  \ started by `<bin`.
  \
  \ }doc

[unneeded] (dbin. dup
?\ need <bin need (d.
?\ : (dbin. ( d n -- ) <bin (d. bin> type space ;

  \ doc{
  \
  \ (dbin. ( d n -- )
  \
  \ Display _d_ as an unsigned binary number with _n_ digits.
  \
  \ See also: `(hex.`, `32bin.`, `16bin.`, `8bin.`, `bin.`.
  \
  \ }doc

[unneeded] 32bin.
?\ need (dbin.  : 32bin. ( d -- ) #32 (dbin. ;

  \ doc{
  \
  \ 32bin. ( d -- )
  \
  \ Display _d_ as an unsigned 32-bit binary number.
  \
  \ See also: `32hex.`, `16bin.`, `8bin.`, `bin.`, `binary`.
  \
  \ }doc

[unneeded] 16bin.
?\ need (dbin.  : 16bin. ( n -- ) s>d #16 (dbin. ;

  \ doc{
  \
  \ 16bin. ( n -- )
  \
  \ Display _n_ as an unsigned 16-bit binary number.
  \
  \ See also: `16bin.`, `32bin.`, `8bin.`, `bin.`, `binary`.
  \
  \ }doc

[unneeded] 8bin.
?\ need (dbin.  : 8bin. ( b -- ) s>d  #8 (dbin. ;

  \ doc{
  \
  \ 8bin. ( n -- )
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

  \ vim: filetype=soloforth
