  \ flow.bracket-switch.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ SwiftForth's `[switch`, an extensible alternative to
  \ `case`.

  \ ===========================================================
  \ Authors

  \ Rick VanNorman. Original code for SwiftForth was published
  \ on Forth Dimensions (volume 20, number 3, pages 19..22,
  \ 1998-09).
  \
  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( switcher :switch <switch )

  \ Raw version, without syntactic sugar

unneeding switcher unneeding :switch and ?( need link@

: switcher ( i*x n a -- j*x )
  dup cell+ @ >r \ save default xt
  begin  link@ ?dup while ( n a )
    2dup cell+ @ = if \ match
      nip cell+ cell+ perform rdrop exit
    then
  repeat r> execute ; ?)

  \ doc{
  \
  \ switcher ( i*x n a -- j*x )
  \
  \ Search the linked list from its head _a_ for a match to the
  \ value _n_. If a match is found, discard _n_ and execute the
  \ associated matched _xt_. If no match is found, leave _n_ on
  \ the stack and execute the default _xt_.
  \
  \ ``switcher`` is a common factor of `:switch` and `[switch`,
  \ two variants of the same control structure.
  \
  \ Origin: SwiftForth.
  \
  \ }doc

unneeding <switch unneeding :switch and ?(

need pick need link,

: <switch ( a xt n -- a ) 2 pick link, , , ; ?)

  \ doc{
  \
  \ <switch ( a xt n -- a ) "start-switch"
  \
  \ Define a new clause of a `:switch` structure whose head is
  \ _a_ to execute _xt_ when the key _n_ is matched.
  \
  \ The switch clauses are 3-cell structures:

  \ . Link to the previous clause of the switch
  \ . Key
  \ . Execution token

  \ Origin: SwiftForth.
  \
  \ }doc

unneeding :switch ?( need switcher need <switch

: :switch ( xt "name" -- a )
  create >mark swap , does> ( n -- ) ( n dfa ) switcher ; ?)

  \ doc{
  \
  \ :switch ( xt "name" -- a ) "colon-switch"
  \
  \ Create a code switch _name_ whose default action is given
  \ by _xt_. Leave the address _a_ of the head of its list on
  \ the stack.
  \
  \ The head _a_ of the switch structure is the address of a
  \ 2-cell structure, with the following contents:

  \ . Link to the last clause of the switch
  \ . Execution token of the default action

  \ Usage example:

  \ ----
  \ : one   ( -- )   ." unu " ;
  \ : two   ( -- )   ." du "  ;
  \ : three ( -- )   ." tri " ;
  \   \ clauses of the switch
  \
  \ : many  ( n -- ) . ." is too much! " ;
  \   \ default action of the switch
  \
  \ ' many :switch .number
  \
  \   ' one   1 <switch
  \   ' two   2 <switch
  \   ' three 3 <switch drop
  \
  \ cr 1 .number 2 .number 3 .number 4 .number
  \
  \ ' .number >body  :noname  ." kvar " ; 4 <switch drop
  \   \ add a new nameless clause for number 4
  \
  \ cr 1 .number 2 .number 3 .number 4 .number
  \
  \ ----

  \ NOTE: `[switch` is the syntactic-sugar variant of
  \ ``:switch``.
  \
  \ Origin: SwiftForth.
  \
  \ See: `<switch`, `[switch`, `switcher`.
  \
  \ }doc

( [+switch [switch runs run: )

  \ Complete version, with syntactic sugar

unneeding [+switch

?\ need >body : [+switch ( "name" -- a ) ' >body ;

  \ doc{
  \
  \ [+switch ( "name" -- a ) "bracket-plus-switch"
  \
  \ Open the `[switch` structure _name_ to include additional
  \ clauses.  The default behavior remains unchanged. The
  \ additions, like the original clauses, are terminated by
  \ `switch]`.  Leave the head _a_ of the given `[switch`
  \ _name_, for clauses to append to.
  \
  \ Origin: SwiftForth.
  \
  \ See: `runs`, `run`.
  \
  \ }doc

unneeding [switch ?( need switcher

: [switch ( "name1" "name2" -- a )
  create >mark ' , does> ( n -- ) ( n dfa ) switcher ;

  \ doc{
  \
  \ [switch ( "name1" "name2" -- a ) "bracket-switch"
  \
  \ Start the definition of a switch structure _name1_
  \ consisting of a linked list of single-precision numbers and
  \ associated behaviors, with its default action _name2_.  The
  \ head _a_ of the switch is left on the stack for defining
  \ clauses.  The switch definition will be terminated by
  \ `switch]`, and can be extended by `[+switch`.
  \
  \ Usage example:

  \ ----
  \ : one   ( -- )   ." unu " ;
  \ : two   ( -- )   ." du "  ;
  \ : three ( -- )   ." tri " ;
  \   \ clauses
  \
  \ : many  ( n -- ) . ." is too much! " ;
  \   \ default action
  \
  \ [switch .number many
  \   1 runs one  2 runs two  3 runs three  switch]
  \
  \ cr 1 .number 3 .number 4 .number
  \
  \ : four  ." kvar " ;
  \
  \ [+switch .number  4 runs four  switch]
  \   \ add a new clause for number 4
  \
  \ cr 1 .number 3 .number 4 .number
  \
  \ [+switch .number  5 run: ." kvin" ;  switch]
  \   \ add a new unnamed clause for number 5
  \
  \ cr 1 .number 4 .number 5 .number
  \ ----

  \ NOTE: ``[switch`` is the syntactic-sugar variant of
  \ `:switch`.
  \
  \ Origin: SwiftForth.
  \
  \ See: `runs`, `run:`.
  \
  \ }doc

: switch] ( a -- ) drop ; ?)

  \ doc{
  \
  \ switch] ( a -- ) "switch-bracket"
  \
  \ Terminate a switch structure (or the latest additions to
  \ it) by marking the end of its linked list.  Discard the
  \ switch head _a_ from the stack.
  \
  \ Origin: SwiftForth.
  \
  \ See: `[switch`, `[+switch`, `runs`, `run:`.
  \
  \ }doc

unneeding runs ?( need <switch

: runs ( a n "name" -- a ) ' swap <switch ; ?)

  \ doc{
  \
  \ runs ( a n "name" -- )
  \
  \ Add a clause to a `[switch` structure whose head is _a_.
  \ The key value of the clause is _n_ and its associated
  \ behavior is the previously defined _name_.
  \
  \ Origin: SwiftForth.
  \
  \ See: `[switch`, `switch]`.
  \
  \ }doc

unneeding run: ?( need evaluate need <switch need :noname

: run: ( a n "ccc<semicolon>" -- a )
  :noname ';' parse evaluate postpone ; ( xt )
  swap <switch ; ?)

  \ doc{
  \
  \ run: ( a n "ccc<semicolon>" -- a ) "run-colon"
  \
  \ Add a clause to a `[switch` structure whose head is _a_.
  \ The key value of the clause is _n_ and its associated
  \ behavior is one or more previously defined words, ending
  \ with `;`.
  \
  \ Origin: SwiftForth.
  \
  \ See: `switch]`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Adapt the original code.
  \
  \ 2016-04-24: Add `need :noname` and `need pick`, because
  \ those words have been moved from the kernel to the library.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-05-14: Update: `evaluate` has been moved to the
  \ library.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-25: Fix authorship and description. Fix and improve
  \ the documentation after the SwiftForth manual and Forth
  \ Dimensions (volume 20, number 3, pages 19..22). Separate
  \ the raw version from the syntactic sugar. Move the test to
  \ <meta.test.misc.fsb>.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-12-10: Fix change log.
  \
  \ 2017-12-10: Improve documentation. Improve needing. Unalias
  \ `switch]`.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
