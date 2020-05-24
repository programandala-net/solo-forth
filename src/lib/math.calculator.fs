  \ math.calculator.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202005241406
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ROM calculator support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Move the stack and make it configurable. The
  \ default location is limited by the small free memory left
  \ to BASIC.
  \
  \ XXX FIXME -- When the calculator stack is out of bounds,
  \ the calculator could issue a BASIC error and crash the
  \ system. Test it.
  \
  \ XXX TODO -- Add more control structures.
  \
  \ XXX TODO -- Test everything.
  \
  \ XXX TODO -- Improve documentation: Add more details to some
  \ calculator commands.

( calculator )

need alias

wordlist constant calculator-wordlist

  \ doc{
  \
  \ calculator-wordlist  ( -- wid )
  \
  \ The word list that contains the `calculator` commands.
  \
  \ }doc

: calculator ( -- ) calculator-wordlist >order $C5 c, $EF c, ;

  \ doc{
  \
  \ calculator ( -- )
  \
  \ Start compilation of ROM calculator commands: Add
  \ `calculator-wordlist` to the search `order` and compile the
  \ following assembly instructions to start the ROM
  \ calculator:

  \ ----
  \ push bc ; save the Forth IP
  \ rst $28 ; call the ROM calculator
  \ ----

  \ See: `end-calculator`.
  \
  \ }doc

calculator-wordlist >order
get-current  calculator-wordlist set-current

: end-calc ( -- ) $38 c, ;

  \ doc{
  \
  \ end-calc ( -- )
  \
  \ Compile the ``end-calc`` ROM `calculator` command:

  \ ----
  \ db $38 ; exit the ROM calculator
  \ ----

  \ See: `end-calculator`.
  \
  \ }doc

: end-calculator ( -- ) previous end-calc $C1 c, ;

  \ doc{
  \
  \ end-calculator ( -- )
  \
  \ Stop compiling ROM `calculator` commands: Restore the
  \ search order and compile the following assembly
  \ instructions to exit the ROM calculator:

  \ ----
  \ db $38 ; ``end-calc`` ROM calculator command
  \ pop bc ; restore the Forth IP
  \ ----

  \ See: `end-calc`.
  \
  \ }doc

-->

( calculator )

: |+ ( -- ) $0F c, ;

  \ doc{
  \
  \ |+ ( -- ) "bar-plus"
  \
  \ Compile the ``addition`` ROM `calculator` command.
  \
  \ See: `|-`.
  \
  \ }doc

: |- ( -- ) $03 c, ;

  \ doc{
  \
  \ |- ( -- ) "bar-minus"
  \
  \ Compile the ``subtract`` ROM `calculator` command.
  \
  \ See: `|+`.
  \
  \ }doc

: |* ( -- ) $04 c, ;

  \ doc{
  \
  \ |* ( -- ) "bar-star"
  \
  \ Compile the ``multiply`` ROM `calculator` command.
  \
  \ See: `|/`, `|**`.
  \
  \ }doc

: |/ ( -- ) $05 c, ;

  \ doc{
  \
  \ |/ ( -- ) "bar-slash"
  \
  \ Compile the ``division`` ROM `calculator` command.
  \
  \ See: `|mod`, `|*`.
  \
  \ }doc

: |mod ( -- ) $32 c, ;

  \ doc{
  \
  \ |mod ( -- ) "bar-mod"
  \
  \ Compile the ``n-mod-m`` ROM `calculator` command.
  \
  \ See: `|/`.
  \
  \ }doc

: |** ( -- ) $06 c, ;

  \ doc{
  \
  \ |** ( -- ) "bar-star-star"
  \
  \ Compile the ``to-power`` ROM `calculator` command.
  \
  \ See: `|sqrt`, `|*`.
  \
  \ }doc

: |sqrt ( -- ) $28 c, ;

  \ doc{
  \
  \ |sqrt ( -- ) "bar-s-q-r-t"
  \
  \ Compile the ``sqr`` ROM `calculator` command.
  \
  \ See: `|**`.
  \
  \ }doc

-->

( calculator )

: |negate ( -- ) $1B c, ;

  \ doc{
  \
  \ |negate ( -- ) "bar-negate"
  \
  \ Compile the ``negate`` ROM `calculator` command.
  \
  \ See: `|abs`, `|sgn`.
  \
  \ }doc

: |sgn ( -- ) $29 c, ;

  \ doc{
  \
  \ |sgn ( -- ) "bar-s-g-n"
  \
  \ Compile the ``sgn`` ROM `calculator` command.
  \
  \ See: `|abs`, `|negate`.
  \
  \ }doc

: |abs ( -- ) $2A c, ;

  \ doc{
  \
  \ |abs ( -- ) "bar-abs"
  \
  \ Compile the ``abs`` ROM `calculator` command.
  \
  \ See: `|sgn`, `|int`, `|truncate`.
  \
  \ }doc

: |int ( -- ) $27 c, ;

  \ doc{
  \
  \ |int ( -- ) "bar-int"
  \
  \ Compile the ``int`` ROM `calculator` command.
  \
  \ See: `|abs`, `|truncate`.
  \
  \ }doc

: |truncate ( -- ) $3A c, ;

  \ doc{
  \
  \ |truncate ( -- ) "bar-truncate"
  \
  \ Compile the ``truncate`` ROM `calculator` command.
  \
  \ See: `|abs`, `|int`.
  \
  \ }doc

: |re-stack ( r -- r' ) $3D c, ;

  \ doc{
  \
  \ |re-stack ( r -- r' ) "bar-re-stack"
  \
  \ Compile the ``re-stack`` ROM `calculator` command.
  \
  \ }doc

: |0 ( -- ) $A0 c, ;

  \ doc{
  \
  \ |0 ( -- ) "bar-zero"
  \
  \ Compile the ROM calculator command that stacks 0.
  \
  \ See: `|half`, `|1`, `|10`, `|pi2/`.
  \
  \ }doc

: |1 ( -- ) $A1 c, ;

  \ doc{
  \
  \ |1 ( -- ) "bar-one"
  \
  \ Compile the ROM calculator command that stacks 1.
  \
  \ See: `|0`, `|half`, `|10`, `|pi2/`.
  \
  \ }doc

: |half ( -- ) $A2 c, ;

  \ doc{
  \
  \ |half ( -- ) "bar-half"
  \
  \ Compile the ROM calculator command that stacks 1/2.
  \
  \ See: `|0`, `|1`, `|10`, `|pi2/`.
  \
  \ }doc

: |pi2/ ( -- ) $A3 c, ;

  \ doc{
  \
  \ |pi2/ ( -- ) "bar-pi-two-slash"
  \
  \ Compile the ROM calculator command that stacks pi/2.
  \
  \ See: `|0`, `|half`, `|1`, `|10`, `|acos`, `|asin`,
  \ `|atan`, `|sin`, `|cos`, `|tan`.
  \
  \ }doc

: |10 ( -- ) $A4 c, ;

  \ doc{
  \
  \ |10 ( -- ) "bar-ten"
  \
  \ Compile the ROM calculator command that stacks 10.
  \
  \ See: `|0`, `|half`, `|1`, `|pi2/`.
  \
  \ }doc

-->

( calculator )

: |ln ( -- ) $25 c, ;

  \ doc{
  \
  \ |ln ( -- ) "bar-l-n"
  \
  \ Compile the ``ln`` ROM `calculator` command.
  \
  \ See: `|exp`.
  \
  \ }doc

: |exp ( -- ) $26 c, ;

  \ doc{
  \
  \ |exp ( -- ) "bar-exp"
  \
  \ Compile the ``exp`` ROM `calculator` command.
  \
  \ See: `|ln`.
  \
  \ }doc

: |acos ( -- ) $23 c, ;

  \ doc{
  \
  \ |acos ( -- ) "bar-a-cos"
  \
  \ Compile the ``acos`` ROM `calculator` command.
  \
  \ See: `|asin`, `|atan`, `|cos`, `|sin`, `|tan`,
  \ `|pi2/`.
  \
  \ }doc

: |asin ( -- ) $22 c, ;

  \ doc{
  \
  \ |asin ( -- ) "bar-a-sin"
  \
  \ Compile the ``asin`` ROM `calculator` command.
  \
  \ See: `|acos`, `|atan`, `|cos`, `|sin`, `|tan`,
  \ `|pi2/`.
  \
  \ }doc

: |atan ( -- ) $24 c, ;

  \ doc{
  \
  \ |atan ( -- ) "bar-a-tan"
  \
  \ Compile the ``atan`` ROM `calculator` command.
  \
  \ See: `|acos`, `|asin`, `|cos`, `|sin`, `|tan`,
  \ `|pi2/`.
  \
  \ }doc

: |cos ( -- ) $20 c, ;

  \ doc{
  \
  \ |cos ( -- ) "bar-cos"
  \
  \ Compile the ``cos`` ROM `calculator` command.
  \
  \ See: `|acos`, `|asin`, `|atan`, `|sin`, `|tan`,
  \ `|pi2/`.
  \
  \ }doc

: |sin ( -- ) $1F c, ;

  \ doc{
  \
  \ |sin ( -- ) "bar-sin"
  \
  \ Compile the ``sin`` ROM `calculator` command.
  \
  \ See: `|acos`, `|asin`, `|atan`, `|cos`, `|tan`,
  \ `|pi2/`.
  \
  \ }doc

: |tan ( -- ) $21 c, ;

  \ doc{
  \
  \ |tan ( -- ) "bar-tan"
  \
  \ Compile the ``tan`` ROM `calculator` command.
  \
  \ See: `|acos`, `|asin`, `|atan`, `|cos`, `|sin`,
  \ `|pi2/`.
  \
  \ }doc

-->

( calculator )


: |drop ( -- ) $02 c, ;

  \ doc{
  \
  \ |drop ( -- ) "bar-drop"
  \
  \ Compile the ``delete`` ROM `calculator` command.
  \
  \ See: `|dup`, `|swap`, `|over`, `|2dup`.
  \
  \ }doc

: |dup ( -- ) $31 c, ;

  \ doc{
  \
  \ |dup ( -- ) "bar-dup"
  \
  \ Compile the ``duplicate`` ROM `calculator` command.
  \
  \ See: `|drop`, `|swap`, `|over`, `|2dup`.
  \
  \ }doc

: |swap ( -- ) $01 c, ;

  \ doc{
  \
  \ |swap ( -- ) "bar-swap"
  \
  \ Compile the ``exchange`` ROM `calculator` command.
  \
  \ See: `|drop`, `|dup`, `|over`, `|2dup`.
  \
  \ }doc

: |>mem ( n -- ) $C0 + c, ;

  \ doc{
  \
  \ |>mem ( n -- ) "bar-to-mem"
  \
  \ Compile the ``st-mem`` ROM `calculator` command for memory
  \ number _n_ (0..5).
  \
  \ NOTE: ``st-mem`` copies the floating-point stack TOS to the
  \ the calculator memory number _n_, but does not remove it
  \ from the floating-point stack.
  \
  \ }doc

: |mem> ( n -- ) $E0 + c, ;

  \ doc{
  \
  \ |mem> ( n -- ) "bar-mem-to"
  \
  \ Compile the ``get-mem`` ROM `calculator` command for memory
  \ number _n_ (0..5).
  \
  \ }doc

: |over ( -- ) 2 |>mem |drop 1 |>mem 2 |mem> 1 |mem> ;

  \ doc{
  \
  \ |over ( -- ) "bar-over"
  \
  \ Compile the ROM calculator commands to do `over`, using
  \ `|>mem` and `|mem>` (calculator memory positions 1 and 2
  \ are used).
  \
  \ See: `|drop`, `|dup`, `|swap`, `|2dup`.
  \
  \ }doc

: |2dup ( -- ) 2 |>mem |drop 1 |>mem |drop
               1 |mem> 2 |mem> 1 |mem> 2 |mem> ;

  \ doc{
  \
  \ |2dup ( -- ) "bar-two-dup"
  \
  \ Compile the ROM `calculator` commands to do `2dup`, using
  \ `|>mem` and `|mem>` (calculator memory positions 1 and 2
  \ are used).
  \
  \ See: `|drop`, `|dup`, `|swap`, `|over`.
  \
  \ }doc

-->

( calculator )


: |0= ( -- ) $30 c, ;

  \ doc{
  \
  \ |0= ( -- ) "bar-zero-equals"
  \
  \ Compile the ``not`` ROM `calculator` command.
  \
  \ See: `|0<`, `|0>`, `|=`, `|<>`, `|>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |0< ( -- ) $36 c, ;

  \ doc{
  \
  \ |0< ( -- ) "bar-zero-less"
  \
  \ Compile the ``less-0`` ROM `calculator` command.
  \
  \ See: `|0=`, `|0>`, `|=`, `|<>`, `|>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |0> ( -- ) $37 c, ;

  \ doc{
  \
  \ |0> ( -- ) "bar-zero-greater"
  \
  \ Compile the ``greater-0`` ROM `calculator` command.
  \
  \ See: `|0=`, `|0<`, `|=`, `|<>`, `|>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

-->

( calculator )

-->  \ XXX TMP -- ignore this block

  \ XXX FIXME -- These commands always return true.
  \
  \ 2016-04-20:
  \
  \ After some research, it seems the reason is the numbers are
  \ compared as strings.  Some commands of the ROM calculator
  \ are used to compare numbers and strings, and the routine
  \ checks the parameters before doing the comparison.
  \
  \ Somehow the ROM routine at $353B gets confused because the
  \ command is not restored from $5C67 (the BREG system
  \ variable).
  \
  \ I examined the source of the ROM calculator and followed
  \ its execution using the debugger of the Fuse emulator, in
  \ BASIC and Forth. So far I got the following clues:
  \
  \ $335E: the command in B is saved to $5C67. This is at the
  \ start of the calculator, so it doesn't makes sense the
  \ first time, because the command is not in B. In Forth, B
  \ contains the high part of the IP ($78 at the time of
  \ writing). But this address is a re-entry point, forced by
  \ the calculator by manipulating the Z80 stack.
  \
  \ $336C: the command is in A, ok.
  \
  \ $338C: the command is modified for indexing, ok.
  \
  \ $339D: the command should be restored by `ld bc,($5C66)`,
  \ which is the low part of STKEND and the high part of BREG.
  \ The register B should contain the command, but not right
  \ after the first entry into the calculator.
  \
  \ $33A1: BREG is in B, which in Forth is $78, bad.
  \
  \ $33A1: Restore the ROM calculator literal: `ld a,($5C67)`.
  \ This is not executed by Forth's `f=`, but it is when the
  \ BASIC command `print 1=1` is interpreted.
  \
  \ $353B: B contains $78, not the command. The routine does a
  \ string comparison. But in BASIC, at this point register B
  \ contains the command.

: |= ( -- ) $0E c, ;

  \ doc{
  \
  \ |= ( -- ) "bar-equals"
  \
  \ Compile the ``nos-eql`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|<>`, `|>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |<> ( -- ) $0B c, ;

  \ doc{
  \
  \ |<> ( -- ) "bar-not-equals"
  \
  \ Compile the ``nos-neql`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|=`, `|>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |> ( -- ) $0C c, ;

  \ doc{
  \
  \ |> ( -- ) "bar-greater"
  \
  \ Compile the ``no-grtr`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|=`, `|<>`, `|<`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |< ( -- ) $0D c, ;

  \ doc{
  \
  \ |< ( -- ) "bar-less"
  \
  \ Compile the ``no-less`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|=`, `|<>`, `|>`, `|<=`,
  \ `|>=`.
  \
  \ }doc

: |<= ( -- ) $09 c, ;

  \ doc{
  \
  \ |<= ( -- ) "bar-less-equals"
  \
  \ Compile the ``no-l-eql`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|=`, `|<>`, `|>`, `|<`,
  \ `|>=`.
  \
  \ }doc

: |>= ( -- ) $0A c, ;

  \ doc{
  \
  \ |>= ( -- ) "bar-greater-equals"
  \
  \ Compile the ``no-gr-eql`` ROM `calculator` command.
  \
  \ WARNING: This calculator command doesn't work fine when
  \ used from Forth. See its source file for details.
  \
  \ See: `|0=`, `|0<`, `|0>`, `|=`, `|<>`, `|>`, `|<`,
  \ `|<=`.
  \
  \ }doc

-->

( calculator )

: |?branch ( -- ) $00 c, ;

  \ doc{
  \
  \ |?branch ( -- ) "bar-question-branch"
  \
  \ Compile the ``jump-true`` ROM `calculator` command.
  \
  \ See: `|0branch`, `|branch`.
  \
  \ }doc

: |0branch ( -- ) |0= |?branch ;

  \ doc{
  \
  \ |0branch ( -- ) "bar-zero-branch"
  \
  \ Compile ROM `calculator` commands `|0=` and `|?branch` to
  \ do a jump when the TOS of the calculator stack is zero.
  \
  \ See: `|branch`, `|?branch`.
  \
  \ }doc

: |branch ( -- ) $33 c, ;

  \ doc{
  \
  \ |branch ( -- ) "bar-branch"
  \
  \ Compile the ``jump`` ROM `calculator` command.
  \
  \ See: `|0branch`, `|?branch`.
  \
  \ }doc

-->

( calculator )

: |>mark ( -- a ) here 0 c, ;

  \ doc{
  \
  \ |>mark ( -- a ) "bar-greater-mark"
  \
  \ Compile space for the displacement of a ROM `calculator`
  \ forward branch which will later be resolved by `|>resolve`.
  \
  \ Typically used before either `|branch`, `|?branch` or
  \ `|0branch`.
  \
  \ }doc

: |from-here ( a -- n ) here swap - ;

  \ doc{
  \
  \ |from-here ( a -- n ) "bar-from-here"
  \
  \ Calculate the displacement _n_ from the current data-space
  \ pointer to address _a_. Used by `|>resolve` and
  \ `|<resolve`.
  \
  \ }doc

: |>resolve ( a -- ) dup |from-here swap c! ;

  \ doc{
  \
  \ |>resolve ( orig -- ) "bar-to-resolve"
  \
  \ Resolve a ROM `calculator` forward branch by storing the
  \ displacement from _orig_ to the current position into
  \ _orig_, which was left by `|>mark`.
  \
  \ }doc

' here alias |<mark ( -- dest )

  \ doc{
  \
  \ |<mark ( -- dest ) "bar-from-mark"
  \
  \ Leave the address _dest_ of the current data-space pointer
  \ as the destination of a ROM `calculator` backward branch
  \ which will later be resolved by `|<resolve`.
  \
  \ Typically used before either `|branch`, `|?branch` or
  \ `|0branch`.
  \
  \ }doc

: |<resolve ( dest -- ) |from-here c, ;

  \ doc{
  \
  \ |<resolve ( dest -- ) "bar-from-resolve"
  \
  \ Resolve a ROM `calculator` backward branch by compiling the
  \ displacement from the current position to address _dest_,
  \ which was left by `|<mark`.
  \
  \ }doc

: |if ( -- a ) |0branch |>mark ;

  \ doc{
  \
  \ |if ( -- orig ) "bar-if"
  \
  \ Compile a ROM `calculator` conditional `|0branch` and put
  \ the address _orig_ of its destination address on the stack,
  \ to be resolved by `|else` or `|then`.
  \
  \ }doc

: |else ( orig1 -- orig2 ) |branch |>mark swap |>resolve ;

  \ doc{
  \
  \ |else ( orig1 -- orig2 ) "bar-else"
  \
  \ Put the location of a new unresolved forward reference
  \ _orig2_ onto the stack, to be resolved by `|then`.  Resolve
  \ the forward reference _orig1_, left by `|if`.
  \
  \ }doc

' |>resolve alias |then ( orig -- )

  \ doc{
  \
  \ |then ( orig -- ) "bar-then"
  \
  \ Resolve the forward reference _orig_, left by `|else` or
  \ `|if`.
  \
  \ }doc

set-current  previous

  \ ===========================================================
  \ Change log

  \ 2015-09-23: Start. Main development, as part of the
  \ floating-point module.
  \
  \ 2016-04-11: Revision. Code reorganized. Improvements.
  \
  \ 2016-04-13: Fixes and improvements. First usable version.
  \
  \ 2016-04-18: Extracted the code from the floating-point
  \ module, in order to reuse it. Much improved. Added `if then
  \ else`. Added `int`.
  \
  \ 2016-04-20: Improved `2dup`. Commented out `=`, `<>`, `<`,
  \ `<=`, `>` and `>=`, which cannot be used yet.
  \
  \ 2016-10-28: Fix block title that caused `>=` and other
  \ calculator operators be found by `need` instead of the
  \ integer ones, because this module comes before the integer
  \ operators in the library disk.
  \
  \ 2017-05-06: Add a "|" prefix to all calculator commands.
  \ This makes the code clearer, makes search order changes
  \ unnecessary and makes a single glossary possible.  Update,
  \ improve and complete the documentation.
  \
  \ 2018-02-17: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-04-14: Fix markup in documentation.
  \
  \ 2020-05-05: Fix cros references.

  \ vim: filetype=soloforth
