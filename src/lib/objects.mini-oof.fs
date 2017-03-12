  \ objects.mini-oof.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Gforth's object oriented package "mini-oof".
  \
  \ See Gforth's manual for detailed documentation and usage
  \ examples.

  \ ===========================================================
  \ Authors

  \ Bernd Paysan, 1998.
  \
  \ Integrated into Solo Forth by Marcos Cruz
  \ (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( mini-oof )

need alias

: method ( m v "name" -- m' v )
  create  over , swap cell+ swap
  does> ( ... o -- ... ) @ over @ + @ execute ;

  \ doc{
  \
  \ method ( m v "name" -- m' `)
  \
  \ Define a selector.
  \
  \ }doc

: var ( m v size "name" -- m v' )
  create  over , +  does> ( o -- addr ) ( o pfa ) @ + ;

  \ doc{
  \
  \ var ( m v size "name" -- m v' )
  \
  \ Define a variable with _size_ bytes.
  \
  \ }doc

: class ( class -- class methods vars ) dup 2@ ;

  \ doc{
  \
  \ class ( class -- class methods vars )
  \
  \ Start the definition of a class.
  \
  \ }doc

: end-class ( class methods vars "name" -- )
  create  here >r , dup , 2 cells ?do ['] noop , 1 cells +loop
  cell+ dup cell+ r> rot @ 2 cells /string move ;

  \ doc{
  \
  \ end-class ( class methods vars "name" -- )
  \
  \ End the definition of a class.
  \
  \ }doc

: defines ( xt class "name" -- ) ' >body @ + ! ;

  \ doc{
  \
  \ defines ( xt class "name" -- )
  \
  \ Bind _xt_ to the selector _name_ in class _class_.
  \
  \ }doc

: new ( class -- o ) here over @ allot swap over ! ;

  \ doc{
  \
  \ new ( class -- o )
  \
  \ Create a new incarnation of the class _class_.
  \
  \ }doc

: :: ( class "name" -- ) ' >body @ + @ compile, ;

  \ doc{
  \
  \ :: ( class "name" -- )
  \
  \ Compile the method for the selector _name_ of the class
  \ _class_ (not immediate!).
  \
  \ }doc

create object  1 cells , 2 cells ,

  \ doc{
  \
  \ object ( -- a )
  \
  \ The base class of all objets.
  \
  \ }doc

' noop alias mini-oof

  \ ===========================================================
  \ Change log

  \ 2016-11-19: Copy the original code.
  \
  \ 2016-11-23: Document the words after the Gforth's manual.

  \ vim: filetype=soloforth
