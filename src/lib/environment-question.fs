  \ environment-question.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712092313
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The environmental queries of Forth-2012.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( environment? )

need search-wordlist need alias

wordlist constant environment-wordlist ( -- wid )

  \ doc{
  \
  \ environment-wordlist ( -- wid )
  \
  \ A constant. _wid_ is the identifier of the word list where
  \ the environmental queries are defined.
  \
  \ See: `environment?`.
  \
  \ }doc

: environment? ( ca len -- false | i*x true )
  environment-wordlist search-wordlist
  if  execute true  else  false  then ;

  \ doc{
  \
  \ environment? ( ca len -- false | i*x true )
  \
  \ The string _ca len_ is the identifier of an environmental
  \ query. If the string is not recognized, return a false
  \ flag. Otherwise return a true flag and some information
  \ about the query.
  \

  \ [cols="16,8,8,32" caption="Environmental Query Strings"]
  \ |===
  \ | String   | Value data type  | Constant? | Meaning

  \ | /COUNTED-STRING    | n      | yes       |  maximum size of a counted string, in characters
  \ | /HOLD              | n      | yes       |  size of the pictured numeric output string buffer, in characters
  \ | /PAD               | n      | yes       |  size of the scratch area pointed to by PAD, in characters
  \ | ADDRESS-UNIT-BITS  | n      | yes       |  size of one address unit, in bits
  \ | FLOORED            | flag   | yes       |  true if floored division is the default
  \ | MAX-CHAR           | u      | yes       |  maximum value of any character in the implementation-defined character set
  \ | MAX-D              | d      | yes       |  largest usable signed double number
  \ | MAX-N              | n      | yes       |  largest usable signed integer
  \ | MAX-U              | u      | yes       |  largest usable unsigned integer
  \ | MAX-UD             | ud     | yes       |  largest usable unsigned double number
  \ | RETURN-STACK-CELLS | n      | yes       |  maximum size of the return stack, in cells
  \ | STACK-CELLS        | n      | yes       |  maximum size of the data stack, in cells
  \ |===

  \ Notes:
  \
  \ . Forth-2012 designates the Forth-94 practice of using
  \   `environment?` to inquire whether a given word set is
  \   present as obsolescent.  The Forth-94 environmental strings
  \   are not supported in Solo Forth.
  \ . In Solo Forth environment queries are also independent
  \   ordinary constants accessible by `need`.

  \
  \ Origin: Forth-2012 (CORE).
  \
  \ See: `environment-wordlist`.  `/counted-string`,
  \ `/pad`, `address-unit-bits`, `floored`, `max-char`,
  \ `max-d`, `max-n`, `max-u`, `max-ud`, `return-stack-cells`,
  \ `stack-cells`.
  \
  \ }doc

need address-unit-bits need max-char need /counted-string
need /pad need floored need max-n need max-u need max-d
need max-ud need return-stack-cells need stack-cells

get-current environment-wordlist dup >order set-current

' address-unit-bits alias address-unit-bits ( -- n )
  \ Size of one address unit, in bits.

' max-char alias max-char ( -- u )
  \ Maximum value of any character in the character set.

' /counted-string alias /counted-string ( -- n )
  \ Maximum size of a counted string, in characters.

' /hold alias /hold
  \ Size of the pictured numeric string output buffer, in
  \ characters.

' /pad alias /pad ( -- n )
  \ Size of the scratch area pointed to by `pad`, in
  \ characters.

' floored alias floored ( -- f ) -->
  \ True if floored division is the default.

( environment? )

' max-n alias max-n ( -- n )
  \ Largest usable signed integer.

' max-u alias max-u ( -- u )
  \ Largest usable unsigned integer.

' max-d alias max-d ( -- d )
  \ Largest usable signed double.

' max-ud alias max-ud ( -- ud )
  \ Largest usable unsigned double.

' return-stack-cells alias return-stack-cells ( -- n )
    \ Maximum size of the return stack, in cells.

' stack-cells alias stack-cells ( -- n )
    \ Maximum size of the data stack, in cells.

  \ XXX TODO -- add "#locals" when needed

set-current previous

( address-unit-bits max-char /counted-string /pad floored )

[unneeded] address-unit-bits

?\ 8 cconstant address-unit-bits ( -- n )

  \ doc{
  \
  \ address-unit-bits ( -- n )
  \
  \ _n_ is the size of one address unit, in bits.
  \
  \ See: `max-char`, `environment?`.
  \
  \ }doc

[unneeded] max-char

?\ 255 cconstant max-char ( -- u )

  \ doc{
  \
  \ max-char  ( -- u )
  \
  \ _u_ is the maximum value of any character in the character
  \ set.
  \
  \ See: `address-unit-bits`, `/counted-string`,
  \ `environment?`.
  \
  \ }doc

[unneeded] /counted-string

?\ 255 cconstant /counted-string ( -- n )

  \ doc{
  \
  \ /counted-string ( -- n )
  \
  \ _n_ is the maximum size of a counted string, in characters.
  \
  \ See: `max-char`, `environment?`.
  \
  \ }doc

[unneeded] /pad

?\ 84 cconstant /pad ( -- n )

  \ doc{
  \
  \ /pad ( -- n )
  \
  \ _n_ is the size of the scratch area pointed to by `pad`, in
  \ characters.
  \
  \ See: `/hold`, `environment?`.
  \
  \ }doc

[unneeded] floored

?\ false cconstant floored ( -- f )

  \ doc{
  \
  \ floored ( -- f )
  \
  \ _f_ is _true_ if floored division is the default.
  \
  \ See: `environment?`.
  \
  \ }doc

( max-n max-u max-d max-ud return-stack-cells stack-cells )

[unneeded] max-n

?\ 32767 constant max-n ( -- n )

  \ doc{
  \
  \ max-n ( -- n )
  \
  \ _n_ is the largest usable signed integer.
  \
  \ See: `max-u`, `max-d`, `environment?`.
  \
  \ }doc

  \ XXX REMARK -- Equivalent calculation:
  \   0 1 2 um/mod nip 1-

[unneeded] max-u

?\ -1 constant max-u ( -- u )

  \ doc{
  \
  \ max-u ( -- u )
  \
  \ _u_ is the largest usable unsigned integer.
  \
  \ See: `max-n`, `max-ud`, `environment?`.
  \
  \ }doc

[unneeded] max-d

?\ need max-n -1 max-n 2constant max-d ( -- d )

  \ doc{
  \
  \ max-d ( -- d )
  \
  \ _d_ is the largest usable signed double.
  \
  \ See: `max-n`, `max-ud`, `environment?`.
  \
  \ }doc

[unneeded] max-ud

?\ -1. 2constant max-ud ( -- ud )

  \ doc{
  \
  \ max-ud ( -- ud )
  \
  \ _ud_ is the largest usable unsigned double.
  \
  \ See: `max-u`, `max-d`, `environment?`.
  \
  \ }doc

[unneeded] return-stack-cells

?\ $2C +origin @ constant return-stack-cells ( -- n )

  \ doc{
  \
  \ return-stack-cells ( -- n )
  \
  \ _n_ is the maximum size of the return stack, in cells.
  \
  \ See: `return-stack-cells`, `environment?`.
  \
  \ }doc

[unneeded] stack-cells

?\ $2A +origin @ constant stack-cells ( -- n )

  \ doc{
  \
  \ return-stack-cells ( -- n )
  \
  \ _n_ is the maximum size of the data stack, in cells.
  \
  \ See: `stack-cells`, `environment?`.
  \
  \ }doc

  \ XXX TODO -- add "#locals" when needed

  \ ===========================================================
  \ Change log

  \ 2015-11-13: Start: only the Forth-2012 queries, not the
  \ obsolescent word set queries of Forth-94.
  \
  \ 2016-05-18: Update: use `wordlist` instead of `vocabulary`,
  \ which has been moved to the library.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2016-12-08: Fix `/hold`, `floored`. Rename the module
  \ filename to <environment-question.fsb>.  Add
  \ `return-stack-cells` and `data-stack-cells`.  Fix restoring
  \ of the search order. Document `environment?` and
  \ `environment-wordlist`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-30: Use `cconstant` when possible.
  \
  \ 2017-03-31: Fix and update the "/hold" query, after
  \ converting the kernel's `/hold` to a constant. Make all
  \ queries accessible as constants, then make aliases in
  \ `environment-wordlist`. This is more versatile, because the
  \ application can `need` individual constants instead of
  \ redefining them or needing `environment?`.
  \
  \ 2017-04-01: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-12-09: Improve documentation.

  \ vim: filetype=soloforth
