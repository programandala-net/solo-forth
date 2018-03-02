  \ flow.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803011813
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Miscellaneous control flow structures that can be defined
  \ in less than one block.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( +perform base-execute call don't executions )

[unneeded] +perform

?\ : +perform ( a n -- ) cells + perform ;

  \ doc{
  \
  \ +perform ( a n -- ) "plus-perform"
  \
  \ Execute the execution token pointed by an offset of _n_
  \ cells from base address _a_, i.e., execute the contents of
  \ element _n_ of the cell table that starts at _a_.
  \
  \ If the execution token is zero, do nothing.
  \
  \ See: `perform`, `execute`, `array>`.
  \
  \ }doc

[unneeded] base-execute

?\ : base-execute ( xt n -- ) base @ >r execute r> base ! ;

  \ Credit:
  \
  \ Word from Gforth.

  \ doc{
  \
  \ base-execute ( xt n -- )
  \
  \ Execute _xt_ with the content of `base` being _n_
  \ and restoring the original `base` afterwards.
  \
  \ }doc

[unneeded] call ?(

code call ( a -- )
  E1 c, C5 c, CD c, >mark C1 c, DD c, 21 c, next , jpnext,
                    >resolve E9 c, end-code ?)

  \   pop hl
  \   push bc
  \   call call_hl
  \   pop bc
  \   ld ix,next
  \   _jp_next
  \ call_hl:
  \   jp (hl)

  \ doc{
  \
  \ call ( a -- )
  \
  \ Call a machine code subroutine at _a_.
  \
  \ See: `execute-hl,`, `call-xt,`.
  \
  \ }doc

[unneeded] don't ?(

: don't ( n1 n2 -- | n1 n2 )
  2dup = if 2drop unnest unnest then ; compile-only ?)

  \ doc{
  \
  \ don't ( n1 n2 -- | n1 n2 )
  \
  \ If _n1_ equals _n2_, remove them and exit the definition
  \ that called ``don't``, else leave _n1 n2_ on the stack.
  \
  \ ``don't`` is a `compile-only` word.
  \
  \ ``don't`` is intended to be used before `do`, as an
  \ alternative to `?do`, when the do-loop structure is
  \ factored in its own word.
  \
  \ Usage example:

  \ ----
  \ : (.range) ( n1 n2 -- ) don't do i . loop ;
  \ : .range ( n1 n2 -- ) (.range) ;
  \ ----

  \ ``don't`` is superseded by the standard word `?do`.
  \
  \ }doc

[unneeded] executions ?( need 2rdrop

  \ Credit:
  \
  \ Code from Galope (module times.fs).

: executions ( xt n -- )
  2>r begin  2r@ while 2r> 1- 2>r execute
      repeat drop 2rdrop ; ?)

  \ doc{
  \
  \ executions ( xt n -- )
  \
  \ Execute _xt_ _n_ times.
  \
  \ See: `times`, `dtimes`.
  \
  \ }doc

( ?repeat 0repeat recurse ?? )

[unneeded] ?repeat ?( need cs-dup need 0until

: ?repeat
  \ Compilation: ( dest -- dest )
  \ Run-time:    ( f -- )
  cs-dup postpone 0until ; immediate compile-only ?)

  \ Credit:
  \
  \ Adapted from the documentation of Forth-2012 and Forth-94.

  \ doc{
  \
  \ ?repeat "question-repeat"
  \   Compilation: ( dest -- dest )
  \   Run-time:    ( f -- )

  \
  \ An alternative exit point for `begin` ... `until` loops: If
  \ _f_ is non-zero, continue execution at `begin`, otherwise
  \ continue execution after `until`.
  \
  \ ``?repeat`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( -- )
  \     begin
  \       ...
  \     flag ?repeat  \ Go back to ``begin`` if flag is non-zero
  \       ...
  \     flag 0repeat  \ Go back to ``begin` if flag is zero
  \       ...
  \     flag until    \ Go back to ``begin`` if flag is false
  \     ...
  \   ;
  \ ----

  \ See: `0repeat`.
  \
  \ }doc

[unneeded] 0repeat ?( need cs-dup

: 0repeat
  \ Compilation: ( dest -- dest )
  \ Run-time:    ( f -- )
  cs-dup postpone until ; immediate compile-only ?)

  \ Credit:
  \
  \ Adapted from the documentation of Forth-2012 and Forth-94.

  \ doc{
  \
  \ 0repeat "zero-repeat"
  \   Compilation: ( dest -- dest )
  \   Run-time:    ( f -- )
  \
  \ An alternative exit point for `begin` ... `until` loops: If
  \ _f_ is zero, continue execution at `begin`, otherwise
  \ continue execution after `until`.
  \
  \ ``0repeat`` is an `immediate` word.
  \
  \ Usage example:

  \ ----
  \ : test ( -- )
  \     begin
  \       ...
  \     flag 0repeat  \ Go back to `begin` if flag is zero
  \       ...
  \     flag ?repeat  \ Go back to `begin` if flag is non-zero
  \       ...
  \     flag until    \ Go back to `begin` if flag is false
  \     ...
  \   ;
  \ ----

  \ See: `?repeat`.
  \
  \ }doc

[unneeded] recurse

?\ : recurse ( -- ) latestxt compile, ; immediate compile-only

  \ doc{
  \
  \ recurse ( -- )
  \
  \ Append the execution semantics of the current definition to
  \ the current definition.
  \
  \ ``recurse`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-83 (Controlled Reference Words), Forth-94
  \ (CORE), Forth-2012 (CORE).
  \
  \ }doc

[unneeded] ?? ?(

: ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  postpone if
  defined ( nt | 0 ) ?dup 0= -13 ?throw
  name>immediate? ( xt f ) if  execute  else  compile,  then
  postpone then
  ; immediate compile-only ?)

  \ Credit:
  \
  \ Original code by Neil Bawd, presented at FORML 1986.

  \ The original code was written two ways:

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   s" if" evaluate  bl word count evaluate
  \   s" then" evaluate
  \ ;  immediate

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   postpone if bl word count evaluate  postpone then
  \ ;  immediate

  \ XXX OLD -- This first version used `postpone` and
  \ `compile,` instead of `evaluate`.

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   postpone if
  \   parse-name find-name 0= -13 ?throw compile,
  \   postpone then
  \ ;  immediate

  \ XXX OLD -- simpler:

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \  postpone if  ' compile,  postpone then
  \ ;  immediate

  \ XXX OLD -- even simpler:

  \ : ?? ( f -- ) 0= if  r> cell+ >r  then ; compile-only

  \ Final version, after a comment by Anton Ertl in
  \ comp.lang.forth, 2015-10-19:

  \ XXX TODO -- 2016-11-26: don't compile `if then` for
  \ immediate _name_; compile `drop` instead?

  \ XXX TODO -- 2016-11-26: It seems more useful the old
  \ version, extended as the rest of alternative conditionals:
  \
  \ : ??  ( f -- )  0= if  r> cell+ >r  then ; compile-only
  \ : 0?? ( f -- )     if  r> cell+ >r  then ; compile-only
  \ : -?? ( f -- ) 0>= if  r> cell+ >r  then ; compile-only
  \ : +?? ( f -- )  0< if  r> cell+ >r  then ; compile-only

  \ doc{
  \
  \ ?? "question-question"
  \   Compilation: ( "name" -- )
  \   Run-time:    ( f -- )
  \

  \ ``??`` is an `immediate` and `compile-only` word.
  \
  \ Compilation:
  \
  \ Parse _name_ and search the current search order for it.
  \ If not found, throw exception #-13. If found and it's an
  \ `immediate` word, execute it, else compile it.
  \
  \ Run-time:
  \
  \ If _f_ is not zero, execute _name_, which was compiled.
  \
  \ }doc

( retry ?retry ?leave )

  \ Description:
  \
  \ Unconditional and conditional branches to the start of the
  \ current definition.

  \ Credit:

  \ Based on the article "RETRY, EXIT, and Word-Level
  \ Factoring", by Richard Astle, published on Forth Dimensions
  \ (volume 17, number 4, page 19, 1995-11).

[unneeded] retry ?( need name>body

: retry ( -- )
  latest name>body postpone again ; immediate compile-only ?)

  \ doc{
  \
  \ retry ( -- )
  \
  \ Do an unconditional branch to the start of the word.
  \
  \ ``retry`` is an `immediate` and `compile-only` word.
  \
  \ See: `?retry`.
  \
  \ }doc

[unneeded] ?retry ?( need retry

: ?retry
  \ Compilation: ( -- )
  \ Run-time: ( f -- )
  postpone if postpone retry postpone then
  ; immediate compile-only ?)

  \ doc{
  \
  \ ?retry "question-retry"
  \   Compilation: ( -- )
  \   Run-time:    ( f -- )
  \
  \ Do a conditional branch to the start of the word.
  \
  \ ``?retry`` is an `immediate` and `compile-only` word.
  \
  \ See: `retry`, `?repeat`, `0repeat`.
  \
  \ }doc

[unneeded] ?leave ?(

code ?leave ( f -- ) ( R: loop-sys -- | loop-sys )
  E1 c, 78 04 + c, B0 05 + c, C2 c, ' leave , jpnext,
  \ pop hl
  \ ld a,h
  \ or l
  \ jp nz,leave_
  \ _jp_next
  end-code ?)

  \ doc{
  \
  \ ?leave ( f -- ) ( R: loop-sys -- | loop-sys ) "question-leave"
  \
  \ If _f_ is non-zero, discard the loop-control parameters for
  \ the current nesting level and continue execution
  \ immediately following the innermost syntactically enclosing
  \ `loop` or `+loop`.
  \
  \ See: `leave`, `unloop`, `do`, `?do`.
  \
  \ }doc

( cond thens orif andif )

  \ Credit of the `cond thens` structure, `orif` and `andif`:
  \
  \ Subject: Re: Multiple WHILE's
  \ From: Wil Baden <neil...@earthlink.net>
  \ Newsgroups: comp.lang.forth
  \ Message-ID: <260620020959020469%neilbawd@earthlink.net>
  \ Date: Wed, 26 Jun 2002 16:58:18 GMT
  \
  \ The usage of `cs-mark` and `cs-test` was borrowed from:
  \
  \ Control-Flow Stack Extensions
  \ http://dxforth.netbay.com.au/cfsext.html

[unneeded] cond ?( need cs-mark need thens

: cond
  \ Compilation: ( C: -- cs-mark )
  \ Run-time:    ( -- )
  cs-mark ; immediate compile-only ?)

  \ doc{
  \
  \ cond
  \   Compilation: ( C: -- cs-mark )
  \   Run-time:    ( -- )

  \
  \ Compilation: Mark the start of a ``cond`` .. `thens`
  \ structure.  Leave _cs-mark_ on the control-flow stack, to
  \ be checked by `thens`.
  \
  \ Run-time: Continue execution.
  \
  \ ``cond`` is an `immediate` and `compile-only` word.
  \
  \ Generic usage example:

  \ ----
  \ : test ( x -- )
  \   cond
  \     test1 if action1 else
  \     test2 if action2 else
  \     test3 if action3 else
  \     default-action
  \   thens ;
  \ ----

  \ Note: The tested value must be preserved and discarded by
  \ the application. Example:

  \ ----
  \ : test ( ca len -- )
  \   cond
  \     2dup s" first"  str= if 2drop ." unua"  else
  \     2dup s" second" str= if 2drop ." dua"   else
  \     2dup s" third"  str= if 2drop ." tria"  else
  \     2dup s" fourth" str= if 2drop ." kvara" else
  \     type ." ?"
  \   thens ;
  \ ----

  \ See: `case`, `cs-mark`, `andif`, `orif`.
  \
  \ }doc

[unneeded] thens ?( need cs-test

: thens
  \ Compilation: ( C: cs-mark orig#1 .. orig#n -- )
  \ Run-time:    ( -- )
  begin cs-test while postpone then repeat drop
  ; immediate compile-only ?)

  \ doc{
  \
  \ thens
  \   Compilation: ( C: cs-mark orig#1 .. orig#n -- )
  \   Run-time:    ( -- )

  \
  \ Compilation: Resolve all forward references _orig#1 ..
  \ orign#n_ with `then` until _0_ is found.
  \
  \ Run-time: Continue execution.
  \
  \ ``thens`` is an `immediate` and `compile-only` word.
  \
  \ ``thens`` is a factor of `endcase` and other control
  \ structures, but it's also the end of the `cond` ..
  \ ``thens`` structure. See `cond` for an usage example.
  \
  \ See: `cs-mark`, `cs-test`, `andif`, `orif`.
  \
  \ }doc

[unneeded] andif ?(

: andif
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( f -- )
  postpone dup postpone if postpone drop
  ; immediate compile-only ?)

  \ doc{
  \
  \ andif "and-if"
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( f -- )
  \
  \ Short-circuit `and` variant of `if`.
  \
  \ ``andif`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : the-end? ( -- f ) cond  won-battle?     andif
  \                           found-treasure? andif
  \                           kill-dragon?    andif
  \                     thens ;
  \ ----

  \ Compare with the following equivalent code, where all three
  \ conditions are always checked:

  \ ----
  \ : the-end? ( -- f ) won-battle?
  \                     found-treasure? and
  \                     kill-dragon?    and ;
  \ ----

  \ See: `orif`, `cond`, `thens`.
  \
  \ }doc

[unneeded] orif ?(

: orif
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( f -- )
  postpone dup postpone 0= postpone if postpone drop
  ; immediate compile-only ?)

  \ doc{
  \
  \ orif "or-if"
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( f -- )
  \
  \ Short-circuit `or` variant of `if`.
  \
  \ ``orif`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : is-alphanum? ( c -- f ) cond  dup is-lower? orif
  \                                 dup is-upper? orif
  \                                 dup is-digit?
  \                           thens nip ;
  \ ----

  \ Compare with the following equivalent code, where all three
  \ conditions are always checked:

  \ ----
  \ : is-alphanum? ( c -- f ) dup  is-lower?
  \                           over is-upper? or
  \                           swap is-digit? or ;
  \ ----

  \ See: `andif`, `cond`, `thens`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ --------------------------------------------
  \ Old

  \ 2015-06-05: Write `recurse` in the kernel.
  \
  \ 2015-11-07: First version of `retry`.
  \
  \ 2016-03-04: Copy the code of `?repeat` from the Forth-2012
  \ documentation.
  \
  \ 2016-04-17: Move `recurse` to the library.
  \
  \ 2016-04-24: Fix `recurse` with `latestxt`: now it can be used
  \ in words created with `:noname`.
  \
  \ 2016-04-26: Document `retry`.
  \
  \ 2016-04-28: Fix the stack notation of `?repeat`.

  \ --------------------------------------------
  \ New

  \ 2016-11-26: Create this module to combine the modules that
  \ contain small control flow structures, in order to save
  \ blocks: <flow.base-execute.fsb>, <flow.call.fsb>,
  \ <flow.dont.fsb>, <flow.executions.fsb>,
  \ <flow.question-question.fsb>, <flow.question-repeat.fsb>,
  \ <flow.recurse.fsb>, <flow.retry.fsb>.
  \
  \ 2016-11-26: Document `base-execute`.  Compact `call` and
  \ document it. Document `??`. Move `?leave` from the kernel.
  \
  \ 2016-12-03: Fix needing of `retry`.
  \
  \ 2016-12-04: Add `+perform`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`. Improve `?repeat`. Fix needing of `??`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-11-27: Move `cond` and `thens` from <flow.select.fs>
  \ and document them. Improve documentation.
  \
  \ 2017-11-28: Improve source layout of `executions`.
  \
  \ 2017-12-02: Improve documentation.
  \
  \ 2017-12-03: Improve documentation.
  \
  \ 2017-12-11: Update `cond` and `thens` with `cs-mark` and
  \ `cs-test`. Update the change log with the old changes of
  \ the former modules. Rewrite `?repeat`. Add `0repeat`.
  \ Improve documentation.
  \
  \ 2018-01-03: Fix documentation layout.
  \
  \ 2018-01-04: Improve documentation. Add `andif` and `orif`.
  \
  \ 2018-02-01: Improve documentation of `cond` and `thens`.
  \
  \ 2018-02-04: Fix documentation layout. Improve
  \ documentation: add pronunciation to words that need it.
  \
  \ 2018-03-01: Fix undesired links in documentation of
  \ `0repeat`.

  \ vim: filetype=soloforth
