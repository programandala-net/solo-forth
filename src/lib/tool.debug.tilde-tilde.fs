  \ tool.debug.tilde-tilde.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `~~` debugging tool.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2019, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ~~ )

need :noname need defer need .s need columns need .name
need 2variable

variable ~~? ~~? on  create ~~y 0 c,

  \ doc{
  \
  \ ~~? ( -- a ) "tilde-tilde-question"
  \
  \ A `variable`. _a_ is the address of a cell containing a flag.
  \ When the flag is true, the debugging code compiled by `~~`
  \ is executed, else ignored.  Its default value is true.
  \
  \ }doc

  \ doc{
  \
  \ ~~y ( -- ca ) "tilde-tilde-y"
  \
  \ A `cvariable`. _ca_ is the address of a character
  \ containing the row the debugging information compiled by
  \ `~~` will be printed at.  Its default value is zero.
  \
  \ }doc

create ~~resume-key bl c,  create ~~quit-key 'q' c,

  \ doc{
  \
  \ ~~quit-key ( -- ca ) "tilde-tilde-quit-key"
  \
  \ A `cvariable`. _ca_ is the address of a character
  \ containing the key code used to quit at the debugging
  \ points compiled by `~~`. If its value is not zero,
  \ `~~control` will wait for a key press in order to quit the
  \ debugging.  Its default value is the code of 'q'.
  \
  \ See also: `~~resume-key`.
  \
  \ }doc

  \ doc{
  \
  \ ~~resume-key ( -- ca ) "tilde-tilde-resume-key"
  \
  \ A `cvariable`. _ca_ is the address of a character
  \ containing the key code used to resume execution at the
  \ debugging points compiled by `~~`.  If ``~~resume-key``
  \ contains zero, `~~control` will not wait for a key.  If
  \ ``~~resume-key`` contains $FF, `~~control` will wait for
  \ any key.  Otherwise `~~control` will wait for the key
  \ stored at ``~~resume-key``, whose default value is `bl`,
  \ the code of the space character.
  \
  \ See also: `~~quit-key`.
  \
  \ }doc

: (~~info ( nt line block -- )
  0 ~~y c@ 2dup 2>r at-xy columns 2* spaces 2r@ at-xy
  ." Block " 4 .r ."  Line " 2 .r space .name 2r> 1+ at-xy .s ;

  \ doc{
  \
  \ (~~info ( -- ) "paren-tilde-tilde-info"
  \
  \ Default action of `~~info`: Show the debugging info
  \ compiled by `~~` and the current contents of the data
  \ stack. At least to lines are used, depending on the
  \ contents of the stack. The first line shows the block, line
  \ and definition name where `~~` was compiled; the second
  \ line shows the contents of the stack. The printing position
  \ can be configured with `~~y`.
  \
  \ }doc

defer ~~info ( nt line block -- )  ' (~~info ' ~~info defer!

  \ doc{
  \
  \ ~~info ( -- ) "tilde-tilde-info"
  \
  \ Show the debugging info compiled by `~~` and the current
  \ contents of the data stack. ``~~info`` is a deferred word
  \ (see `defer`) whose default action is `(~~info`.
  \
  \ }doc

: ~~control? ( -- f ) ~~resume-key c@ ~~quit-key c@ or ;

  \ doc{
  \
  \ ~~control? ( -- f ) "tilde-tilde-control-question"
  \
  \ Is there any key to be checked by `~~control`?
  \
  \ ``~~control?`` is part of the `~~` tool.
  \
  \ }doc

: ~~press? ( c ca -- f ) c@ tuck = swap 0<> and ;

  \ doc{
  \
  \ ~~press? ( c ca -- f ) "tilde-tilde-press-question"
  \
  \ Is the character stored at _ca_ not zero and equal to _c_?
  \ ``~~press?`` is a factor of `~~control` used to check key
  \ presses, in the code compiled by `~~`.
  \
  \ }doc

: ~~control ( -- )
  ~~control? 0= ?exit                       begin key dup
      ~~quit-key ~~press? if drop quit then
    ~~resume-key c@ $FF = if drop exit then
    ~~resume-key ~~press? if exit      then again ; -->


  \ doc{
  \
  \ ~~control ( -- ) "tilde-tilde-control"
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
  \ ~~xy-backup ( -- a ) "tilde-tilde-x-y-backup"
  \
  \ A `2variable`. _a_ is the address of a double cell
  \ that holds cursor coordinates saved and restored by the
  \ default actions of `~~before-info` and `~~after-info`.
  \
  \ ``~~xy-backup`` is part of the `~~` tool.
  \
  \ }doc

defer ~~before-info ( -- )  defer ~~after-info ( -- )

  \ doc{
  \
  \ ~~before-info ( -- ) "tilde-tilde-before-info"
  \
  \ Executed at the start of the debugging code compiled by
  \ `~~`.  ``~~before-info`` is a deferred word (see `defer`).
  \ Its default action is `~~save-xy`, which saves the cursor
  \ coordinates.
  \
  \ See also: `~~after-info`, `~~save-xy`.
  \
  \ }doc

  \ doc{
  \
  \ ~~after-info ( -- ) "tilde-tilde-after-info"
  \
  \ Executed at the end of the debugging code compiled by `~~`.
  \ ``~~after-info`` is a deferred word (see `defer`). Its
  \ default action is `~~restore-xy`, which restores the cursor
  \ coordinates.
  \
  \ See also: `~~before-info`, `~~restore-xy`.
  \
  \ }doc

: ~~save-xy ( -- ) xy ~~xy-backup 2! ;

' ~~save-xy ' ~~before-info defer!

  \ doc{
  \
  \ ~~save-xy ( -- ) "tilde-tilde-save-x-y"
  \
  \ Save the cursor coordinates.  ``~~save-xy`` is the default
  \ action of `~~before-info`.
  \
  \ ``~~save-xy`` is part of the `~~` tool.
  \
  \ See also: `~~restore-xy`, `~~after-info`, `~~xy-backup`.
  \
  \ }doc

: ~~restore-xy ( -- ) ~~xy-backup 2@ at-xy ;

' ~~restore-xy ' ~~after-info defer!

  \ doc{
  \
  \ ~~restore-xy ( -- ) "tilde-tilde-restore-x-y"
  \
  \ Restore the cursor coordinates.  ``~~restore-xy`` is the
  \ default action of `~~after-info`.
  \
  \ ``~~restore-xy`` is part of the `~~` tool.
  \
  \ See also: `~~save-xy`, `~~before-info`, `~~xy-backup`.
  \
  \ }doc

: (~~ ( nt line block -- )
  ~~? @ if    ~~before-info ~~info ~~control ~~after-info
        else  2drop drop  then ;

  \ doc{
  \
  \ (~~ ( nt n u -- ) "paren-tilde-tilde"
  \
  \ The runtime action compiled by `~~` during the definition
  \ of word _nt_ in line _n_ of block _u_:
  \
  \ If the content of `~~?` is not zero, execute the following
  \ words in the given order: `~~before-info`, `~~info`,
  \ `~~control` and `~~after-info`.
  \
  \ See also: `~~y`.
  \
  \ }doc

: ~~ ( -- )
  latest      ( nt )    postpone literal
  >in @ c/l / ( line )  postpone literal
  blk @       ( block ) postpone literal
                        postpone (~~ ; immediate compile-only

  \ doc{
  \
  \ ~~ ( -- ) "tilde-tilde"
  \
  \ Compile the name token, block and line of the current
  \ definition, and `(~~`.
  \
  \ ``~~`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Gforth.
  \
  \ See also: `(~~`, `~~?`, `~~y`, `~~quit-key`, `~~resume-key`,
  \ `~~info`, `~~control` `~~before-info`, `~~after-info`.
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
  \ default actions of `~~save` and `~~after-info` to named
  \ words, in order to make them easier to reuse.
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
  \ 2016-12-24: Improve `~~info` to produce clearer output. Set
  \ default quit and resume keys: `q` and space. Remove `~~x`,
  \ which is not useful because two full lines are used.
  \
  \ 2017-01-17: Improve documentation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action". Update
  \ cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-16: Convert cell variables to character variables.
  \ Change the behaviour of `~~control`: check if
  \ `~~resume-key` contains $FF instead of a negative value.
  \ Update and improve documentation.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-04-11: Update notation "double variable" to
  \ "double-cell variable".
  \
  \ 2018-04-14: Fix documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names. Link `variable` and `2variable` in documentation.
  \
  \ 2019-03-18: Rename `~~save` `~~before-info`. Rename
  \ `~~restore` `~~after-info`. Remove `~~app-info` and make
  \ `~~info` deferred: this is simpler and equally
  \ configurable. Improve documentation.
  \
  \ 2020-05-04: Fix cross reference in `(~~`.
  \
  \ 2020-05-08: Update requirements: `.name` has been moved
  \ from the kernel to the library.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.
  \
  \ 2020-06-15: Improve documentation: Add cross-references to
  \ `cvariable`; replace "This is" with the corresponding word.
  \
  \ 2020-07-28: Improve documentation of deferred words.

  \ vim: filetype=soloforth
