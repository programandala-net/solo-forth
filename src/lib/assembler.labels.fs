  \ assembler.labels.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804152059
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Assembler labels.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( l: )

get-order get-current
only forth definitions need array> need c+!
assembler-wordlist dup >order set-current need ?rel

%01000000 cconstant rl-id  %10000000 cconstant al-id

  \ doc{
  \
  \ rl-id ( -- b ) "r-l-i-d"
  \
  \ _b_ is the identifier of relative references created by
  \ `rl#`.  ``rl-id`` is used as a bitmask added to the label
  \ number stored in `l-refs`.
  \
  \ See: `al-id`.
  \
  \ }doc

  \ doc{
  \
  \ al-id ( -- b ) "a-l-i-d"
  \
  \ _b_ is the identifier of absolute references created by
  \ `al#`.  ``al-id`` is used as a bitmask added to the label
  \ number stored in `l-refs`.
  \
  \ See: `rl-id`.
  \
  \ }doc

create max-labels 8 c,  create max-l-refs 16 c,

  \ doc{
  \
  \ max-labels  ( -- ca )
  \
  \ _ca_ is the address of a byte containing the maximum number
  \ (count) of labels that can be defined by `l:`.  Its default
  \ value is 8, i.e. labels 0..7 can be used. The program can
  \ change the value, but the default one should be restored
  \ after the code word has been compiled.
  \
  \ ``max-labels`` is used by `init-labels` to allocate the
  \ `labels` table.
  \
  \ Usage example:

  \ ----
  \ need assembler need l:
  \ assembler-wordlist >order    max-labels c@
  \                          #24 max-labels c! previous
  \
  \ code my-word ( -- )
  \   \ Z80 code that needs #24 labels
  \ end-code
  \
  \ assembler-wordlist >order max-labels c! previous
  \ ----

  \ See: `max-l-refs`.
  \
  \ }doc

  \ doc{
  \
  \ max-l-refs  ( -- ca )
  \
  \ _ca_ is the address of a byte containing the maximum number
  \ (count) of unresolved label references that can be created
  \ by `rl#` or `al#`.  Its default value is 16. The program
  \ can change the value, but the default one should be
  \ restored after the code word has been compiled.
  \
  \ ``max-l-refs`` is used by `init-labels` to allocate the
  \ `l-refs` table.
  \
  \ Usage example:

  \ ----
  \ need l:
  \ assembler-wordlist >order     max-l-refs c@
  \                           #20 max-l-refs c! previous
  \
  \ code my-word ( -- )
  \   \ Z80 code that needs #20 label references
  \ end-code
  \
  \ assembler-wordlist >order max-l-refs c! previous
  \ ----

  \ See: `max-labels`.
  \
  \ }doc

1 cell+ cconstant /l-ref

  \ doc{
  \
  \ /l-ref ( -- n ) "slash-l-ref"
  \
  \ _n_ is the size in bytes of each label reference stored in
  \ the `l-refs` table.
  \
  \ See: `/l-refs`.
  \
  \ }doc

: /l-refs ( -- n ) max-l-refs c@ /l-ref * ;

  \ doc{
  \
  \ /l-refs ( -- n ) "slash-l-refs"
  \
  \ _n_ is the size in bytes of the `l-refs` table.
  \
  \ See: `max-l-refs`, `/l-ref`, `/labels`.
  \
  \ }doc

: /labels ( -- n ) max-labels c@ cells ;

  \ doc{
  \
  \ /labels ( -- n ) "slash-labels"
  \
  \ _n_ is the size in bytes of the `labels` table.
  \
  \ See: `max-labels`, `/l-refs`.
  \
  \ }doc

variable labels  variable l-refs

  \ doc{
  \
  \ labels  ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the labels table, which is allocated in the
  \ `stringer` by `init-labels`.  The size of the table can be
  \ configured with `max-label`.
  \
  \ Each element of the table (0 index) is one cell, which
  \ contains either the address of the corresponding label or
  \ zero if the label is undefined.
  \
  \ See: `/labels`, `l-refs`.
  \
  \ }doc

  \ doc{
  \
  \ l-refs  ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ address of the label references table, which is allocated
  \ in the `stringer` by `init-labels`. The size of the table
  \ can be configured with `max-l-refs`.
  \
  \ Each element of the table (0 index) has the following
  \ structure:

  \ ....
  \ +0 = byte: unused reference:
  \               all bits are 0
  \            used reference:
  \               label number:        bits 0..5
  \               relative reference?: bit 6 = 1 (mask ``rl-id``)
  \               absolute reference?: bit 7 = 1 (mask ``al-id``)
  \ +1 = cell: label address
  \ ....

  \ }doc

: init-labels ( -- )
  /labels allocate-stringer dup labels ! /labels erase
  /l-refs allocate-stringer dup l-refs ! /l-refs erase ;

init-labels ' init-labels ' init-asm defer!

  \ doc{
  \
  \ init-labels ( -- )
  \
  \ Init the assembler labels and their references, by
  \ allocating space for them in the `stringer` and erasing it.
  \ `labels` and `l-refs` are given new values.
  \
  \ Loading ``init-labels`` makes it the action of `init-asm`,
  \ which is called by `asm` and therefore also by `code` and
  \ `;code`. Therefore, if the program needs a specific ammount
  \ of labels or label references, `max-labels` and
  \ `max-l-refs` must be configured before compiling the
  \ assembly word.
  \
  \ }doc

: ?l# ( n -- ) max-labels c@ 1- u> #-283 ?throw ; -->

  \ doc{
  \
  \ ?l# ( n -- ) "question-l-hash"
  \
  \ If assembler label _n_ is out of range, `throw` exception
  \ #-283.
  \
  \ See: `max-labels`.
  \
  \ }doc

( l: )

: >l-ref ( n -- a ) /l-ref * l-refs @ + ;
  \ Convert label reference _n_ to the address _a_ of its data.

: free-l-ref? ( n -- a f ) >l-ref dup c@ 0= ;

: free-l-ref ( -- a | 0 )
  max-l-refs c@ 0 ?do i free-l-ref? if unloop exit then drop
                  loop 0 ;
  \ Return address _a_ of the first unused label reference in
  \ `l-refs`, or zero if all label references are in use.

: new-l-ref ( orig n b -- )
  or free-l-ref dup 0= #-285 ?throw tuck c! 1+ ! ;
  \ Create a new reference _orig_ to label _n_ of type _b_
  \ (`rl-id` or `al-id`).  If all label references are in use,
  \ throw exception #-285.

: >l ( b -- a )
  [ rl-id al-id or invert ] cliteral and labels @ array> ;

  \ doc{
  \
  \ >l ( b -- a ) "to-l"
  \
  \ _a_ is the address of label _b_ in the `labels` table.
  \
  \ }doc

: resolve-al# ( orig b -- ) >l @ swap ! ;

  \ doc{
  \
  \ resolve-al# ( orig b -- ) "resolve-a-l-hash"
  \
  \ Resolve an absolute reference at _orig_ to label _b_.
  \
  \ See: `resolve-rl#`, `(resolve-ref`, `>l`.
  \
  \ }doc

: resolve-rl# ( orig b -- ) >l @ over 1+ - dup ?rel swap c! ;

  \ doc{
  \
  \ resolve-rl# ( orig b -- ) "resolve-r-l-hash"
  \
  \ Resolve a relative reference at _orig_ to label _b_.
  \
  \ See: `resolve-al#`, `(resolve-ref`, `>l`.
  \
  \ }doc

: (resolve-ref ( orig b -- )
  dup rl-id and if resolve-rl# else resolve-al# then ;
  \ XXX TODO -- Unfactor.

  \ doc{
  \
  \ (resolve-ref ( orig b -- ) "paren-resolve-ref"
  \
  \ Resolve reference at _orig_ to label _b_.
  \
  \ See: `resolve-rl#`, `resolve-al#`.
  \
  \ }doc

: al# ( -- ) here cell- dup @ ( orig n ) dup ?l# dup >l @ ?dup
             if nip swap ! else al-id new-l-ref then ; -->

  \ doc{
  \
  \ al#  ( -- ) "a-l-hash"
  \
  \ Create an absolute reference to an assembler label defined
  \ by `l:`. The label number has been compiled in the last
  \ cell of the latest Z80 instruction.  If the corresponding
  \ label is already defined, its value is patched into the
  \ latest Z80 instruction.  Otherwise it will be patched when
  \ the label is defined by `l:`.
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

( l: )

: rl# ( n -- a ) dup ?l#  dup >l @ ?dup
  if nip else here 1+ dup rot rl-id new-l-ref then ;

  \ doc{
  \
  \ rl#  ( n -- a ) "r-l-hash"
  \
  \ Create a relative reference to assembler label number _n_,
  \ defined by `l:`.  If label _n_ is already defined, _a_ is
  \ its value. Otherwise _a_ is a temporary address to be
  \ consumed by the relative jump instruction, and the actual
  \ address will be resolved when the label is defined by `l:`.
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

: @l-ref ( a -- orig b ) dup 1+ @ swap c@ ;
  \ Fetch a label reference _orig b_ from _a0_.

: resolve-ref ( a -- ) dup @l-ref (resolve-ref /l-ref erase ;
  \ Resolve label reference stored at _a_.

: l-id># ( b -- n ) [ rl-id al-id or invert ] cliteral and ;

: ?resolve-ref ( n1 n2 -- )
  >l-ref dup c@ dup if   l-id># rot = if   resolve-ref
                                      else drop then
                    else 2drop drop then ;
  \ If label reference _n2_ points to label _n1_, resolve it.

: resolve-refs ( n -- )
  max-l-refs c@ 0 ?do dup i ?resolve-ref loop drop ;
  \ Resolve all references to label _n_.

: l! ( x n -- )
  dup >l dup @ #-284 ?throw rot swap ! resolve-refs ;

  \ doc{
  \
  \ l!  ( x n -- ) "l-store"
  \
  \ If assembler label _n_ has been defined in the current
  \ definition, `throw` exception #-284 (assembly label number
  \ already used); else create a new assembler label _n_ with
  \ value _x_ and resolve all previous references to it that
  \ could have been created by `rl#` or `al#`. Usually _x_ is
  \ an address.
  \
  \ See: `l:`.
  \
  \ }doc

: l: ( n -- ) here swap l! ;

  \ doc{
  \
  \ l:  ( n -- ) "l-colon"
  \
  \ If assembler label _n_ has been defined in the current
  \ definition, `throw` exception #-284 (assembly label number
  \ already used); else create a new assembler label _n_ with
  \ the value returned by `here` and resolve all previous
  \ references to it that could have been created by `rl#` or
  \ `al#`.
  \
  \ See: `l!`, `.l`.
  \
  \ }doc

set-current set-order

( .l )

need dump need l:

assembler-wordlist >order

: .l ( -- ) labels @ /labels dump cr l-refs @ /l-refs dump ;

  \ doc{
  \
  \ .l ( -- ) "dot-l"
  \
  \ Dump the contents of the tables pointed by `labels` and
  \ `l-refs`.
  \
  \ ``.l`` is a debugging tool for `assembler` labels defined
  \ by `l:`.
  \
  \ }doc

previous


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
  \ Memory used (including requirements and space for 16 labels
  \ and 16 references):

  \                       Current       Previous (hForth)
  \ Data/code space:      554 B         421 B
  \ Name space:           353 B         145 B

  \ 2017-03-22: Increase number of labels and references to 22.
  \ Fix throw code.
  \
  \ 2017-03-23: Increase number of labels and references to 23,
  \ needed by `dzx7m`. Start an improved version.
  \
  \ 2017-03-24: Reduce the size of the label references table,
  \ by combining the label number and the reference type into
  \ one byte.
  \
  \ 2017-03-25: Make the size of the tables configurable by the
  \ application and use the `stringer` to allocate them.
  \ Memory used (including requirements):

  \ Data/code space:      500 B
  \ Name space:           355 B

  \ Improve documentation.

  \ 2017-03-27: Add `l!` and rewrite `l:` after it.
  \
  \ 2017-05-14: Improve documentation.
  \
  \ 2017-12-06: Improve documentation.
  \
  \ 2018-02-02: Fix layout.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-04-14: Fix documentation of `al-id`.
  \
  \ 2018-04-15: Fix markup in documentation.

  \ vim: filetype=soloforth
