  \ locals.arguments.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modifed: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An implementation of nestable locals, with a predefined set
  \ of ten variables which return their contents.

  \ ===========================================================
  \ Authors

  \ Original code by Marc Perkel, published on Forth Dimensions
  \ (volume 3, number 6, page 185, 1982-03).
  \
  \ Adapted to Solo Forth and improved by Marcos Cruz
  \ (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( toarg +toarg arguments results )

unneeding toarg ?( need arguments ' @ arg-default-action !

: toarg ( -- ) ['] ! arg-action ! ; exit ?)

  \ doc{
  \
  \ toarg ( -- ) "to-arg"
  \
  \ Set the store action for the next local variable created by
  \ `arguments`.
  \
  \ Loading ``toarg`` makes `@` the default action of
  \ `arguments` locals, which is hold in `arg-default-action`.
  \
  \ See: `+toarg`.
  \
  \ }doc

unneeding +toarg ?( need arguments ' @ arg-default-action !

: +toarg ( -- ) ['] +! arg-action ! ; exit ?)

  \ doc{
  \
  \ +toarg ( -- ) "plus-to-arg"
  \
  \ Set the add action for the next local variable. Used with
  \ locals created by `arguments`.
  \
  \ Loading ``+toarg`` makes `@` the default action of
  \ `arguments` locals, which is hold in `arg-default-action`.
  \
  \ See: `toarg`.
  \
  \ }doc

need cell/

variable >args
  \ address of the current arguments in the data stack

variable arg-default-action arg-default-action off

  \ doc{
  \
  \ arg-default-action ( -- a )
  \
  \ A `variable`. _a_ holds the execution token of the default
  \ action performed by the locals defined by `arguments`.  Its
  \ default value is zero, which means "no action" (`noop` can
  \ be used too, but ``arg-default-action off`` is simpler than
  \ ``' noop arg-defaul-action !``).
  \
  \ `toarg` and `+toarg` change the content of
  \ ``arg-default-action``.
  \
  \ The content of ``arg-default-action`` is copied to
  \ `arg-action` by `arguments`, and also every time a local
  \ variable is used.
  \
  \ See: `arg-action`.
  \
  \ }doc

variable arg-action  arg-default-action @ arg-action !

  \ doc{
  \
  \ arg-action ( -- a )
  \
  \ A `variable`. _a_ holds the execution token of the action
  \ performed by the locals defined by `arguments`.  Its
  \ default value is stored in `arg-default-action`.  The
  \ content of ``arg-default-action`` is copied to `arg-action`
  \ by `arguments`, and also every time a local variable is
  \ used.
  \
  \ }doc

: init-arg-action ( -- ) arg-default-action @ arg-action ! ;

  \ doc{
  \
  \ init-arg-action  ( -- )
  \
  \ Set `arg-action` to `arg-default-action`.
  \
  \ ``init-arg-action`` is a factor of `arguments`.
  \
  \ }doc


: arg: ( +n "name" -- )
  create  c,
  does> ( -- x ) ( x -- )
    \ ( dfa | x dfa )
    c@ >args @ swap - arg-action perform init-arg-action ;
  \ Create a new argument _name_ with offset _+n_.

$00 arg: l0 $02 arg: l1 $04 arg: l2 $06 arg: l3 $08 arg: l4
$0A arg: l5 $0C arg: l6 $0E arg: l7 $10 arg: l8 $12 arg: l9 -->

( arguments results )

: arguments ( i*x +n -- j*x )
  r> >args @ >r >r
  cells sp@ + dup >args ! [ 10 cells ] cliteral - sp@ swap -
  cell/ 0 ?do 0 loop init-arg-action ; compile-only

  \ doc{
  \
  \ arguments ( i*x +n -- j*x )
  \
  \ Define the number _+n_ of arguments to take from the stack
  \ and assign them to the first local variables from ``l0`` to
  \ ``l9``. By default, local variables are manipulated with
  \ `@`, `!` and `+!`, like ordinary variables.  They are
  \ returned  with `results`.
  \
  \ Example: The phrase ``3 arguments`` assigns the names of
  \ local variables ``l0`` through ``l9`` to ten stack
  \ positions, with ``l0``, ``l1`` and ``l2`` returning the
  \ address of the top 3 stack values that were there before `3
  \ arguments` was executed. ``l3`` through ``l9`` are
  \ zero-filled and the stack pointer is set to just below
  \ ``l9``.  After all calculating is done, the phrase ``3
  \ results`` leaves that many results on the stack relative to
  \ the stack position when ``arguments`` was executed. All
  \ intermediate stack values are lost, which is good because
  \ you can leave the stack "dirty" and it doesn't matter.
  \
  \ Usage example:

  \ ----
  \ : test ( length width height -- length' volume surface )
  \   3 arguments
  \   l0 @ l1 @ * l5 !       \ surface
  \   l5 @ l2 @ * l4 !       \ volume
  \   $2000 l0 +!            \ length+$2000
  \   l4 @ l1 !              \ volume
  \   l5 @ l2 !              \ surface
  \   3 results ;
  \ ----

  \ When `toarg` or `+toarg` are loaded, they change the
  \ default behaviour of locals: Then ``l0`` through ``l9``
  \ return their contents, not their addresses.  To write them
  \ you precede them with the word `toarg`. For example ``5
  \ toarg l4`` writes a 5 into ``l4``. Execution of ``l4``
  \ returns 5 to the stack. To add a number to a local
  \ variable, you precede it with the word `+toarg`. For
  \ example, ``5 +toarg l4`` adds 5 to the current content of
  \ ``l4``.
  \
  \ Example:

  \ ----
  \ need toarg  need +toarg
  \
  \ : test ( length width height -- length' volume surface )
  \   3 arguments
  \   l0 l1 * toarg l5       \ surface
  \   l5 l2 * toarg l4       \ volume
  \   $2000 +toarg l0        \ add $2000 to length
  \   l4 toarg l1            \ volume
  \   l5 toarg l2            \ surface
  \   3 results ;
  \ ----

  \ The default action of local variables (either return its
  \ address or its value) is hold in `arg-default-action`, as
  \ an execution token.

  \ ``arguments`` is a `compile-only` word.
  \
  \ See: `local`, `anon`.
  \
  \ }doc

: results ( +n -- )
  cells >args @ swap - sp@ - cell/ 0 ?do drop loop
  r> r> >args ! >r ; compile-only

  \ doc{
  \
  \ results ( +n -- )
  \
  \ Define the number _+n_ of local variables to leave on the
  \ stack as results. Used with locals created by `arguments`.
  \
  \ ``results`` is a `compile-only` word.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-14: Start.
  \
  \ 2016-04-09: Fixed, improved, renamed, documented, finished.
  \
  \ 2017-02-17: Fix markup in comment.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-18: Simplify the implementation, using a variable
  \ to hold the action of arguments instead of a table.
  \
  \ The previous implementation (based on an execution table)
  \ used (including requirements) 252 B of data/code space, and
  \ 216 B of name space.
  \
  \ A new experimental implementation with `defer` uses
  \ (including requirements) 258 B of data/code space, 243 B of
  \ name space.
  \
  \ The new implementation with `variable` uses (including
  \ requirements) 256 B of data/code space, and 243 B of name
  \ space. Without `toarg` and `+toarg`, which are optional
  \ now, the used memory is 230 B of data/code space, and 222 B
  \ of name space.
  \
  \ The new implementation holds the default action in a
  \ variable, what makes it easier to use the arguments as
  \ variables, instead of values.
  \
  \ Update and improve the documentation.
  \
  \ 2017-03-19: Complete documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-02-17: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Link `variable` in documentation.

  \ vim: filetype=soloforth
