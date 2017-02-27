  \ screen_mode.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Words that are common to all screen
  \ modes.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-05-07: Compact the blocks.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Make `columns` and `rows` independent for
  \ `need` and document them.  Remove `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-16: Remove `set-font`, which is in the kernel.
  \
  \ 2017-02-17: Update cross references.

( columns rows set-mode-output )

[unneeded] columns
?\ need value  32 value columns  exit

  \ doc{
  \
  \ columns ( -- n )
  \
  \ Return the number of columns in the current screen mode.
  \ The default value is 32.
  \
  \ See also: `rows`, last-column`, `column`.
  \
  \ }doc

[unneeded] rows
?\ need value  24 value rows  exit

  \ doc{
  \
  \ rows ( -- n )
  \
  \ Return the number of rows in the current screen mode.  The
  \ default value is 24.
  \
  \ See also: `columns`, `last-row`, `row`.
  \
  \ }doc

need os-chans

: set-mode-output ( a -- )
  os-chans @ 2dup ! 2dup 5 + ! 15 + ! ;

  \ doc{
  \
  \ set-mode-output ( a -- )
  \
  \ Associate the output routine at _a_ to the system channels
  \ "K", "S" and "P".
  \
  \ }doc
  \ XXX TODO -- why also "P"?

( set-banked-mode-output )

need set-mode-output need >body

0 constant (output-routine)

code (banked-mode-output) ( -- )
  C5 c,  CD c, 0 ,
    \ push bc ; save Forth IP
    \ call output_routine ; to be patched
  here cell- ' (output-routine) >body !
    \ Store the address where the address of the output routine
    \ must be stored, into the constant `(output-routine)`.
  C1 c,  DD c, 21 c, next ,  jpnext, end-code
    \ pop bc ; restore Forth IP
    \ ld ix,next ; restore IX, just in case
    \ jp next

: set-banked-mode-output ( a -- )
  (output-routine) !  \ patch `(banked-mode-output)`
  ['] (banked-mode-output) set-mode-output ;
  \ Associate the output routine at _a_ (which is in the code
  \ bank) to the system channels "K", "S" and "P", using and
  \ intermediate routine to page the code bank in and out.
  \ XXX OLD -- adapt to far memory or remove

  \ vim: filetype=soloforth
