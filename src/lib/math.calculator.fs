  \ math.calculator.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201705062352
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ ROM calculator support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

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
  \ db $38 ; `end-calc` ROM calculator command
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
  \ |+ ( -- )
  \
  \ Compile the ``addition`` ROM `calculator` command.
  \
  \ See: `|-`.
  \
  \ }doc

: |- ( -- ) $03 c, ;

  \ doc{
  \
  \ |- ( -- )
  \
  \ Compile the ``subtract`` ROM `calculator` command.
  \
  \ See: `|+`.
  \
  \ }doc

: |* ( -- ) $04 c, ;

  \ doc{
  \
  \ |* ( -- )
  \
  \ Compile the ``multiply`` ROM `calculator` command.
  \
  \ See: `|\`, `|**`.
  \
  \ }doc

: |/ ( -- ) $05 c, ;

  \ doc{
  \
  \ |/ ( -- )
  \
  \ Compile the ``division`` ROM `calculator` command.
  \
  \ See: `|mod`, `|*`.
  \
  \ }doc

: |mod ( -- ) $32 c, ;

  \ doc{
  \
  \ |mod ( -- )
  \
  \ Compile the ``n-mod-m`` ROM `calculator` command.
  \
  \ See: `|\`.
  \
  \ }doc

: |** ( -- ) $06 c, ;

  \ doc{
  \
  \ |** ( -- )
  \
  \ Compile the ``to-power`` ROM `calculator` command.
  \
  \ See: `|sqrt`, `|*`.
  \
  \ }doc

: |sqrt ( -- ) $28 c, ;

  \ doc{
  \
  \ |sqrt ( -- )
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
  \ |negate ( -- )
  \
  \ Compile the ``negate`` ROM `calculator` command.
  \
  \ See: `|abs`, `|sgn`.
  \
  \ }doc

: |sgn ( -- ) $29 c, ;

  \ doc{
  \
  \ |sgn ( -- )
  \
  \ Compile the ``sgn`` ROM `calculator` command.
  \
  \ See: `|abs`, `|negate`.
  \
  \ }doc

: |abs ( -- ) $2A c, ;

  \ doc{
  \
  \ |abs ( -- )
  \
  \ Compile the ``abs`` ROM `calculator` command.
  \
  \ See: `|sgn`, `|int`, `|truncate`.
  \
  \ }doc

: |int ( -- ) $27 c, ;

  \ doc{
  \
  \ |int ( -- )
  \
  \ Compile the ``int`` ROM `calculator` command.
  \
  \ See: `|abs`, `|truncate`.
  \
  \ }doc

: |truncate ( -- ) $3A c, ;

  \ doc{
  \
  \ |truncate ( -- )
  \
  \ Compile the ``truncate`` ROM `calculator` command.
  \
  \ See: `|abs`, `|int`.
  \
  \ }doc

: |re-stack ( r -- r' ) $3D c, ;

  \ doc{
  \
  \ |re-stack ( r -- r' )
  \
  \ Compile the ``re-stack`` ROM `calculator` command.
  \
  \ }doc

: |0 ( -- ) $A0 c, ;

  \ doc{
  \
  \ |0 ( -- )
  \
  \ Compile the ROM calculator command that stacks 0.
  \
  \ See: `|half`, `|1`, `|10`, `|pi2/`.
  \
  \ }doc

: |1 ( -- ) $A1 c, ;

  \ doc{
  \
  \ |1 ( -- )
  \
  \ Compile the ROM calculator command that stacks 1.
  \
  \ See: `|0`, `|half`, `|10`, `|pi2/`.
  \
  \ }doc

: |half ( -- ) $A2 c, ;

  \ doc{
  \
  \ |half ( -- )
  \
  \ Compile the ROM calculator command that stacks 1/2.
  \
  \ See: `|0`, `|1`, `|10`, `|pi2/`.
  \
  \ }doc

: |pi2/ ( -- ) $A3 c, ;

  \ doc{
  \
  \ |pi2/ ( -- )
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
  \ |10 ( -- )
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
  \ |ln ( -- )
  \
  \ Compile the ``ln`` ROM `calculator` command.
  \
  \ See: `|exp`.
  \
  \ }doc

: |exp ( -- ) $26 c, ;

  \ doc{
  \
  \ |exp ( -- )
  \
  \ Compile the ``exp`` ROM `calculator` command.
  \
  \ See: `|ln`.
  \
  \ }doc

: |acos ( -- ) $23 c, ;

  \ doc{
  \
  \ |acos ( -- )
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
  \ |asin ( -- )
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
  \ |atan ( -- )
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
  \ |cos ( -- )
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
  \ |sin ( -- )
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
  \ |tan ( -- )
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
  \ |drop ( -- )
  \
  \ Compile the ``delete`` ROM `calculator` command.
  \
  \ See: `|dup`, `|swap`, `|over`, `|2dup`.
  \
  \ }doc

: |dup ( -- ) $31 c, ;

  \ doc{
  \
  \ |dup ( -- )
  \
  \ Compile the ``duplicate`` ROM `calculator` command.
  \
  \ See: `|drop`, `|swap`, `|over`, `|2dup`.
  \
  \ }doc

: |swap ( -- ) $01 c, ;

  \ doc{
  \
  \ |swap ( -- )
  \
  \ Compile the ``exchange`` ROM `calculator` command.
  \
  \ See: `|drop`, `|dup`, `|over`, `|2dup`.
  \
  \ }doc

: |>mem ( n -- ) $C0 + c, ;

  \ doc{
  \
  \ |>mem ( n -- )
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
  \ |mem> ( n -- )
  \
  \ Compile the ``get-mem`` ROM `calculator` command for memory
  \ number _n_ (0..5).
  \
  \ }doc

: |over ( -- ) 2 |>mem |drop 1 |>mem 2 |mem> 1 |mem> ;

  \ doc{
  \
  \ |over ( -- )
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
  \ |2dup ( -- )
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
  \ |0= ( -- )
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
  \ |0< ( -- )
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
  \ |0> ( -- )
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
  \ |= ( -- )
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
  \ |<> ( -- )
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
  \ |> ( -- )
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
  \ |< ( -- )
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
  \ |<= ( -- )
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
  \ |>= ( -- )
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
  \ |?branch ( -- )
  \
  \ Compile the ``jump-true`` ROM `calculator` command.
  \
  \ See: `|0branch`, `|branch`.
  \
  \ }doc

: |0branch ( -- ) |0= |?branch ;

  \ doc{
  \
  \ |0branch ( -- )
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
  \ |branch ( -- )
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
  \ |>mark ( -- a )
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
  \ |from-here ( a -- n )
  \
  \ Calculate the displacement _n_ from the current data-space
  \ pointer to address _a_. Used by `|>resolve` and
  \ `|<resolve`.
  \
  \ }doc

: |>resolve ( a -- ) dup |from-here swap c! ;

  \ doc{
  \
  \ |>resolve ( orig -- )
  \
  \ Resolve a ROM `calculator` forward branch by storing the
  \ displacement from _orig_ to the current position into
  \ _orig_, which was left by `|>mark`.
  \
  \ }doc

' here alias |<mark ( -- dest )

  \ doc{
  \
  \ |<mark ( -- dest )
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
  \ |<resolve ( dest -- )
  \
  \ Resolve a ROM `calculator` backward branch by compiling the
  \ displacement from the current position to address _dest_,
  \ which was left by `|<mark`.
  \
  \ }doc

: |if ( -- a ) |0branch |>mark ;

  \ doc{
  \
  \ |if ( -- orig )
  \
  \ Compile a ROM `calculator` conditional `|0branch` and put
  \ the address _orig_ of its destination address on the stack,
  \ to be resolved by `|else` or `|then`.
  \
  \ }doc

: |else ( orig1 -- orig2 ) |branch |>mark swap |>resolve ;

  \ doc{
  \
  \ |else ( orig1 -- orig2 )
  \
  \ Put the location of a new unresolved forward reference
  \ _orig2_ onto the stack, to be resolved by `|then`.  Resolve
  \ the forward reference _orig1_, left by `|if`.
  \
  \ }doc

' |>resolve alias |then ( orig -- )

  \ doc{
  \
  \ |then ( orig -- )
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
  \ `<=`, `>` and `>=`, which can not be used yet.
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

  \ vim: filetype=soloforth
