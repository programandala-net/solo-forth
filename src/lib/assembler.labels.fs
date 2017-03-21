  \ assembler.labels.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703212020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Local labels for the Z80 assembler.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( l: )

get-order get-current
only forth definitions need array> need c+!
assembler-wordlist dup >order set-current need ?rel

1 cconstant rl-id  2 cconstant al-id
  \ Identifier of relative references.
  \ Identifier of absolute references.

#16 cconstant max-label  #16 cconstant max-l-ref

2 cell+ cconstant /l-ref
  \ Size of a label reference.
  \ Structure:
  \ +0 = byte: 0:           unused reference
  \            1 (`rl-id`): relative reference
  \            2 (`al-id`): absolute reference
  \ +1 = byte: label number
  \ +2 = cell: label address

  \ XXX TODO -- Combine byte 0 and 1 into one? Use the last bit
  \ to mark absolute references.

max-label /l-ref * cconstant /l-refs

create l-refs /l-refs allot
  \ Table of label references.  Used references can be
  \ scattered through the table.

max-label cells cconstant /labels

create labels /labels allot
  \ Table of labels. One cell to hold the address of each
  \ label, or zero if the label is undefined.

: init-labels ( -- )
  l-refs /l-refs erase labels /labels erase ; init-labels
  \ Init the labels and the label references, by erasing their
  \ data.

' init-labels ' init-asm defer! -->

( l: )

: ?l# ( n -- ) [ max-label 1- ] 1literal u> #-283 ?throw ;
  \ If assembler label _n_ is out of range, throw exception
  \ #-283.

: >l-ref ( n -- a ) /l-ref * l-refs + ;
  \ Convert label reference _n_ to the address _a_ of its data.

: free-l-ref ( -- a | 0 )
  max-l-ref 0 ?do i >l-ref dup c@ 0= if unloop exit then drop
              loop 0 ;
  \ Return address _a_ of the first unused label reference in
  \ `l-refs`, or zero if all label references are in use.

: ?free-l-ref ( -- a ) free-l-ref dup 0= #285 ?throw ;
  \ Return address _a_ of the first unused label reference in
  \ `l-refs`. If all label references are in use, throw
  \ exception #-285.

: !l-ref ( a n b a0 -- ) tuck c! 1+ tuck c! 1+ ! ;
  \ Store a label reference _a n b_ at _a0_.

: new-l-ref ( a n b -- ) ?free-l-ref !l-ref ;
  \ Create a new reference to label _n_, of type _b_ (`rl-id`
  \ for relative reference, `al-id` for absolute reference), to
  \ be stored at _a_.

: >l ( n -- a ) labels array> ;
  \ Return address _a_ of label _n_ in the labels array.
  \ _a_ holds the value of the label.

: resolve-al# ( a n -- ) >l @ swap ! ;
  \ Resolve an absolute reference to label _n_, and store it at
  \ _a_.

: resolve-rl# ( a n -- ) >l @ over 1+ - dup ?rel swap c! ;
  \ Resolve a relative reference to label _n_ and store it at
  \ _a_.

: resolve-l# ( a n b -- )
  rl-id = if resolve-rl# else resolve-al# then ;
  \ Resolve reference to label _n_, of type _b_ (`rl-id` for
  \ relative reference, `al-id` for absolute reference), and
  \ store it at _a_.

: (l# ( a n b -- )
  over ?l# over >l @ if resolve-l# else new-l-ref then ; -->
  \ Manage a new reference to label _n_, of type _b_ (1 for
  \ relative reference, 2 for absolute reference), to be stored
  \ at _a_. If label _n_ has been defined, resolve the
  \ reference; else store it.

( l: )

: al# ( -- ) here cell- dup @ al-id (l# ;

  \ doc{
  \
  \ al#  ( -- )
  \
  \ Create an absolute reference to an assembler label defined
  \ by `l:` The label number has been compiled in the last cell
  \ of the latest Z80 instruction.  If the corresponding label
  \ is already defined, its value is patched into the latest
  \ Z80 instruction.  Otherwise it will be patched after the
  \ label has been defined by `l:`.
  \
  \ Usage example:

  \ ----
  \ code my-code ( -- )
  \   #2 call, al#  \ a call to label #2
  \   nop,
  \   #2 l:         \ definition of label #2
  \   ret,
  \ end-code
  \ ----

  \ WARNING: ``al#`` is used after the Z80 command, while its
  \ counterpart `rl#` is used before the Z80 command.
  \
  \ }doc

  \ : rl# ( -- ) here 1- dup c@ 2+ $FF and rl-id (l# ;
  \
  \ XXX OLD -- First version, for syntax `#0 jr, rl#`, which
  \ can not work, because the label number is interpreted as an
  \ address by the assembler relative jump instruction, causing
  \ exception #-269 (relative jump too long). That syntax is
  \ needed for absolute references, though, because the length
  \ of the opcode can vary (1 or 2 bytes, before the value of
  \ the label).

: rl# ( n -- a )
  dup ?l#
  dup >l @ ?dup if   nip
                else here 1+ tuck swap rl-id new-l-ref then ;

  \ doc{
  \
  \ rl#  ( n -- a )
  \
  \ Create a relative reference to assembler label number _n_,
  \ defined by `l:`.  If label _n_ is already defined, _a_ is
  \ its value. Otherwise _a_ is a temporary address just to be
  \ consumed by the relative jump instruction, and the actual
  \ address will be resolved after the label has been defined
  \ by `l:`.
  \
  \ Usage example:

  \ ----
  \ code my-code ( -- )
  \   #2 rl# jr, \ a relative jump to label #2
  \   nop,
  \   #2 l:      \ definition of label #2
  \   ret,
  \ end-code
  \ ----

  \ WARNING: ``rl#`` is used before the Z80 command, while its
  \ counterpart `al#` is used after the Z80 command.
  \
  \ }doc

: @l-ref ( a0 -- a n b ) dup 2+ @ swap dup 1+ c@ swap c@ ;
  \ Fetch a label reference _a n b_ from _a0_.

: resolve-ref ( a -- ) dup @l-ref resolve-l# /l-ref erase ;
  \ Resolve label reference stored at _a_.

: resolve-refs ( n -- )
  max-l-ref 0 ?do dup i >l-ref dup 1+ c@ rot =
    if resolve-ref else drop then
  loop drop ;
  \ Resolve all references to label _n_.

: l: ( n -- )
  dup >l dup @ #-284 ?throw here swap ! resolve-refs ;

  \ doc{
  \
  \ l:  ( n -- )
  \
  \ Create a new assembler label _n_.
  \
  \ See also: `rl#`, `al#`.
  \
  \ }doc


set-current set-order

  \ ===========================================================
  \ Change log

  \ 2016-11-14: Code extracted from hForth (version 0.9.7,
  \ 1997-06-02). First changes to adapt the source layout to
  \ the style used in Solo Forth.
  \
  \ 2016-12-06: Revise source layout, comments, stack comments.
  \ Divide source into blocks. Rename `xhere` and `codeb!`
  \ after the hForth documentation. Use `?rel`, which is
  \ already in the assemblers, instead of the original code.
  \ Prepare for future development.
  \
  \ 2016-12-26: Review, try, test. Factor. Integrate init into
  \ `asm`. Replace original error checking with `?throw` and
  \ specific exception codes.
  \
  \ 2017-02-21: Need `?rel`. No need to load the whole
  \ assembler. Improve documentation.
  \
  \ 2017-03-20: Add alternative implementation extracted from
  \ DX-Forth (version 4.15, 2016-01-16).  First changes to
  \ adapt it to Solo Forth.
  \
  \ 2017-03-21: Rewrite the code from scratch to support
  \ relative and absolute references. Remove the previous
  \ implementation based on hForth, and the unfinished
  \ implementation based on DX-Forth.
  \
  \ Memory used (including requirements):
  \
  \                       Current       Previous (hForth)
  \ Data/code space:      554 B         421 B
  \ Name space:           353 B         145 B

  \ vim: filetype=soloforth
