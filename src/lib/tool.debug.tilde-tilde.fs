  \ tool.debug.tilde-tilde.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703132011
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `~~` debugging tool.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ~~ )

need :noname need defer need .s need columns

variable ~~?  ~~? on

  \ doc{
  \
  \ ~~? ( -- a )
  \
  \ A variable that holds a flag. When the flag is true, the
  \ debugging code compiled by `~~` is executed, else ignored.
  \ Its default value is true.
  \
  \ }doc

variable ~~y  ~~y off

  \ doc{
  \
  \ ~~y ( -- a )
  \
  \ A variable that holds the row the debugging information
  \ compiled by `~~` will be printed at.  Its default value is
  \ zero.
  \
  \ }doc

variable ~~quit-key  'q' ~~quit-key !

  \ doc{
  \
  \ ~~quit-key ( -- a )
  \
  \ A variable that holds the key code used to quit at the
  \ debugging points compiled by `~~`. If its value is not
  \ zero, `~~control` will wait for a key press in order to
  \ quit the debugging.  Its default value is the code of 'q'.
  \
  \ See also: `~~resume-key`.
  \
  \ }doc

variable ~~resume-key  bl ~~resume-key !

  \ doc{
  \
  \ ~~resume-key ( -- a )
  \
  \ A variable that holds the key code used to resume execution
  \ at the debugging points compiled by `~~`.  If it contains
  \ zero, `~~control` will not wait for a key.  If it contains
  \ a negative value (e.g. _true_), `~~control` will wait for
  \ any key.  Otherwise `~~control` will wait for the key it
  \ contains.  Its default value is `bl`, the code of the space
  \ character.
  \
  \ See also: `~~quit-key`.
  \
  \ }doc

: ~~info ( nt line block -- )
  0 ~~y @ 2dup 2>r at-xy columns 2* spaces 2r@ at-xy
  ." Block" 4 .r ."  Line" 3 .r space .name 2r> 1+ at-xy .s ;

  \ doc{
  \
  \ ~~info ( -- )
  \
  \ Show the debugging info compiled by `~~` and the current
  \ contents of the data stack. At least to lines are used,
  \ depending on the contents of the stack. The first line
  \ shows the block, line and definition name where `~~`
  \ was compiled; the second line shows the contents of the
  \ stack. The printing position can be configured with
  \ `~~y`.
  \
  \ }doc

: ~~control? ( -- f ) ~~resume-key @ ~~quit-key @ or ;

  \ doc{
  \
  \ ~~control? ( -- f )
  \
  \ Is there any key to be checked by `~~control`?
  \
  \ ``~~control??`` is part of the `~~` tool.
  \
  \ }doc

: ~~press? ( c a -- f ) @ tuck = swap 0<> and ;

  \ doc{
  \
  \ ~~press? ( c a -- f )
  \
  \ Is the contents of _a_ not zero and equal to _c_?
  \ This is a factor of `~~control` used to check key presses,
  \ in the code compiled by `~~`.
  \
  \ }doc

: ~~control ( -- )
  ~~control? 0= ?exit                         begin  key dup
    ~~quit-key ~~press? if  drop quit  then
      ~~resume-key @ 0< if  drop exit  then
  ~~resume-key ~~press? if  exit       then   again ;  -->

  \ doc{
  \
  \ ~~control ( -- )
  \
  \ Keyboard control used by the debug points compiled by `~~`:
  \ If the contents of `~~quit-key` and `~~resume-key` are zero
  \ do nothing, else wait for a key press in an endless loop:
  \ If the pressed key equals the contents of `~~quit-key`,
  \ then execute `quit`; if the pressed key equals the contents
  \ of `~~resume-key`, then exit.
  \
  \ See also: `~~control?`, `~~press?`.
  \
  \ }doc

( ~~ )

2variable ~~xy-backup

  \ doc{
  \
  \ ~~xy-backup ( -- a )
  \
  \ A double variable that holds cursor coordinates saved and
  \ restored by the default actions of `~~save` and
  \ `~~restore`.
  \
  \ ``~~xy-backup`` is part of the `~~` tool.
  \
  \ }doc

defer ~~save ( -- ) defer ~~restore ( -- )

  \ doc{
  \
  \ ~~save ( -- )
  \
  \ Save system status before executing the debugging code
  \ compiled by `~~`..  This is a deferred word. Its default
  \ action is to save the cursor coordinates.
  \
  \ See also: `~~restore`, `~~save-xy`.
  \
  \ }doc

  \ doc{
  \
  \ ~~restore ( -- )
  \
  \ Restore system status after executing the debugging code
  \ compiled by `~~`.  This is a deferred word. Its default
  \ action is to restore the cursor coordinates.
  \
  \ See also: `~~save`, `~~restore-xy`.
  \
  \ }doc

: ~~save-xy ( -- ) xy ~~xy-backup 2! ;

  \ doc{
  \
  \ ~~save-xy ( -- )
  \
  \ Save the cursor coordinates.  This is the default
  \ action of `~~save`.
  \
  \ ``~~save-xy`` is part of the `~~` tool.
  \
  \ See also: `~~restore-xy`, `~~restore`, `~~xy-backup`.
  \
  \ }doc

: ~~restore-xy ( -- ) ~~xy-backup 2@ at-xy ;

  \ doc{
  \
  \ ~~restore-xy ( -- )
  \
  \ Restore the cursor coordinates.  This is the default
  \ action of `~~restore`.
  \
  \ ``~~restore-xy`` is part of the `~~` tool.
  \
  \ See also: `~~save-xy`, `~~save`, `~~xy-backup`.
  \
  \ }doc

' ~~save-xy ' ~~save defer!  ' ~~restore-xy ' ~~restore defer!

defer ~~app-info ( -- ) ' noop ' ~~app-info defer!

  \ doc{
  \
  \ ~~app-info ( -- )
  \
  \ A deferred word that can be used by the application in
  \ order to show application-specific debugging information as
  \ part of the debugging code compiled by `~~`.  By default it
  \ does nothing.
  \
  \ See also: `~~info`.
  \
  \ }doc


: (~~) ( nt line block -- )
  ~~? @ if    ~~save ~~app-info ~~info ~~control ~~restore
        else  2drop drop  then ;

  \ doc{
  \
  \ (~~) ( nt n u -- )
  \
  \ If the content of `~~?` is not zero, execute the debugging
  \ code that was compiled by `~~` during the definition of
  \ word _nt_ in line _n_ of block _u_.
  \
  \ See also: `~~y`, `~~control`.
  \
  \ }doc

: ~~ ( -- )
  latest      ( nt )    postpone literal
  >in @ c/l / ( line )  postpone literal
  blk @       ( block ) postpone literal
                        postpone (~~) ; immediate compile-only

  \ doc{
  \
  \ ~~ ( -- )
  \
  \ Compile debugging code.
  \
  \ ``~~`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Gforth.
  \
  \ See also: `(~~)`, `~~?`, `~~y`, `~~quit-key`, `~~resume-key`,
  \ `~~info`, `~~app-info`, `~~control` `~~save`, `~~restore`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-02-18: First version.
  \
  \ 2016-11-14: Document all words. Use `defer!`, which is the
  \ kernel, instead of `is`, which is in the library.
  \
  \ 2016-11-25: Add a pause control with a configurable key to
  \ resume. Update and improve documentation. Convert the
  \ default actions of `~~save` and `~~restore` to named words,
  \ in order to make them easier to reuse.
  \
  \ 2016-11-29: Improve `~~control` to wait for any key when
  \ `~~resume-key` is less than zero. This makes it possible to
  \ debug some kind of programs without interfering with the
  \ key presses accepted by them.
  \
  \ 2016-12-03: Improve stack comments of `~~`. Rename `~~show`
  \ to `~~info`.
  \
  \ 2016-12-04: Add `~~app-info`. Need `.s` (it's still in the
  \ kernel, but only during development).
  \
  \ 2016-12-24: Improve `~~info` to produce clearer output.
  \ Set default quit and resume keys: `q` and space. Remove
  \ `~~x`, which is not useful because two full lines are used.
  \
  \ 2017-01-17: Improve documentation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.

  \ vim: filetype=soloforth
