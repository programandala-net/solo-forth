  \ flow.bracket-switch.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
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
  \ Forth, 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

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

( switcher :switch <switch )

  \ Raw version, without syntactic sugar

need link@ need link, need pick

: switcher ( i*x n head -- j*x )
  dup cell+ @ >r  \ save default xt
  begin  link@ ?dup while ( n a )
    2dup cell+ @ = if   \ match
      nip cell+ cell+ perform  rdrop exit
    then
  repeat  r> execute ;

  \ doc{
  \
  \ switcher ( i*x n head -- j*x )
  \
  \ Search the linked list from its _head_ for a match to the
  \ value _n_. If a match is found, discard _n_ and execute the
  \ associated matched _xt_. If no match is found, leave _n_ on
  \ the stack and execute the default xt.
  \
  \ See also: `:switch`, `[switch`, `<switch`.
  \
  \ }doc

: :switch ( xt "name" -- head )
  create  >mark swap ,  does> ( n -- ) ( n pfa ) switcher ;

  \ doc{
  \
  \ :switch ( xt "name" -- head )
  \
  \ Create a code switch whose default action is given by
  \ _xt_. Leave the address of the _head_ of its list on the
  \ stack.
  \
  \ The _head_ of the switch structure is the address of a
  \ 2-cell structure:
  \
  \ 1. link (to the last clause of the switch)
  \ 2. default xt
  \
  \ See also: `<switch`, `[switch`.
  \
  \ }doc

: <switch ( head xt n -- head ) 2 pick link,  , , ;

  \ doc{
  \
  \ <switch ( head xt n -- head )
  \
  \ Define a new clause to execute _xt_ when the key _n_
  \ is matched.
  \
  \ The switch clauses are 3-cell structures:
  \
  \ 1. link (to the previous clause of the switch)
  \ 2. key
  \ 3. xt
  \
  \ See also: `:switch`, `[switch`.
  \
  \ }doc

( [+switch [switch switch] runs run: )

  \ Complete version, with syntactic sugar

[unneeded] [+switch
?\ need >body  : [+switch ( "name" -- head ) ' >body ;

  \ doc{
  \
  \ [+switch ( "name" -- head )
  \
  \ Open the switch structure _name_ to include additional
  \ clauses.  The default behavior remains unchanged. The
  \ additions, like the original clauses, are terminated by
  \ `switch]`.  Leave the _head_ of the given switch _name_,
  \ for clauses to append to.
  \
  \ Origin: SwiftForth.
  \
  \ See also: `[switch`, `runs`, `run`.
  \
  \ }doc

[unneeded] [switch [unneeded] switch] and ?( need switcher

: [switch ( "name1" "name2" -- head )
  create  >mark ' ,  does> ( n -- ) ( n pfa ) switcher ;

  \ doc{
  \
  \ [switch ( "name1" "name2" -- head )
  \
  \ Start the definition of a switch structure _name1_
  \ consisting of a linked list of single-precision numbers and
  \ associated behaviors, with its default action _name2_.
  \ The _head_ of the switch is left on the stack for defining
  \ clauses.  The switch definition will be terminated by
  \ `switch]`, and can be extended by `[+switch`.
  \
  \ Origin: SwiftForth.
  \
  \ See also: `runs`, `run:`.
  \
  \ }doc

need alias  ' drop alias switch] ( head -- ) ?)

  \ doc{
  \
  \ switch] ( head -- )
  \
  \ Terminate a switch structure (or the latest additions to
  \ it) by marking the end of its linked list.  Discard the
  \ switch _head_ from the stack.
  \
  \ Origin: SwiftForth.
  \
  \ See also: `[switch`, `[+switch`, `runs`, `run:`.
  \
  \ }doc

[unneeded] runs ?( need <switch
: runs ( head n "name" -- head ) ' swap <switch ; ?)

  \ doc{
  \
  \ runs ( head n "name" -- )
  \
  \ Add a clause to a switch structure _head_.  The key value
  \ of the clause is _n_ and its associated behavior is the
  \ previously defined _name_.
  \
  \ Origin: SwiftForth.
  \
  \ See also: `[switch`, `switch]`.
  \
  \ }doc

[unneeded] run: ?( need evaluate need <switch need :noname
: run: ( head n "ccc<semicolon>" -- head )
  :noname ';' parse evaluate postpone ; ( xt )
  swap <switch ; ?)

  \ doc{
  \
  \ run: ( head n "ccc<semicolon>" -- head )
  \
  \ Add a clause to a switch structure _head_.  The key value
  \ of the clause is _n_ and its associated behavior is one or
  \ more previously defined words, ending with `;`.
  \
  \ Origin: SwiftForth.
  \
  \ See also: `[switch`, `switch]`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Adapt the original code.

  \ vim: filetype=soloforth
