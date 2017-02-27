  \ data.xstack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ `xstack`, an implementation of extra stacks.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.
  \
  \ Code adapted from Galope (xstack module).

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-10-14: Rename `xstack` to `allocate-xstack` and write
  \ `allot-xstack`. Move the common code to its own block.
  \
  \ 2016-11-26: Improve `allot-xstack`.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-21: Replace `xdepth.` with `.depth`. Rename `.x` to
  \ `.xs`.
  \
  \ 2017-01-25: Make all stack operators and tools individually
  \ accessible to `need`. Rename `set-xstack` to `xstack`.
  \ Make `allot-xstack` and `allocate-xstack` more versatile:
  \ they don't parse a name anymore, but simply return the
  \ address of the new stack.
  \
  \ 2017-01-26: Remove `xp@`, `xp!` and `xp+!`.  Fix `xfree`.
  \ Improve and complete documentation of all words.
  \
  \ 2017-02-17: Update cross references.

( xsize xp xp0 xstack xfree allocate-xstack allot-xstack )

[unneeded] xsize  [unneeded] xp and  [unneeded] xp0 and
[unneeded] xstack and ?(

need value

0 value xsize  0 value xp  0 value xp0

  \ doc{
  \
  \ xsize ( -- n )
  \
  \ Size of the current `xstack` in address units.
  \
  \ }doc

  \ doc{
  \
  \ xp ( -- a )
  \
  \ A variable. Address _a_ holds the address of the current
  \ `xstack` pointer.
  \
  \ }doc

  \ doc{
  \
  \ xp0 ( -- a )
  \
  \ Initial address of the current `xstack` pointer.
  \
  \ }doc

: xstack ( a -- )
  dup @ to xp0  cell+ dup to xp  cell+ @ to xsize ; ?)

  \ doc{
  \
  \ xstack ( a -- )
  \
  \ Make the extra stack _a_ the current one. _a_ is the
  \ address returned by `allot-xstack` or `allocate-xstack`
  \ when the extra stack was created.
  \
  \ Extra stacks grow towards high memory.  _a_ is the address
  \ of a table that contains the metadata of the xstack, which
  \ is the following:

  \ ....
  \ +0 = initial value of the stack pointer (1 cell below the
  \      stack space)
  \ +2 = stack pointer
  \ +4 = maximum size in address units
  \ ....

  \ `xp0`, `xp` and `xsize` are used to access the contents of
  \ the table.

  \ }doc

[unneeded] xfree ?\ : xfree ( -- ) xp0 cell+ free throw ;

  \ doc{
  \
  \ xfree ( -- )
  \
  \ Free the space used by the current `xstack`, which was
  \ created by `allocate-xstack`.
  \
  \ }doc

[unneeded] allocate-xstack ?( need allocate

: allocate-xstack ( n -- a )
  cells here >r allocate throw cell- dup , , , r> ; ?)

  \ doc{
  \
  \ allocate-xstack ( n -- a )
  \
  \ Create an `xstack` in the heap. _n_ is the size in
  \ cells.  Return its address _a_.
  \
  \ See also: `xfree`, `allocate-xstack`.
  \
  \ }doc

[unneeded] allot-xstack ?(

: allot-xstack ( n -- a )
  cells dup here dup >r cell+ cell+ dup , , , allot r> ; ?)

  \ doc{
  \
  \ allot-xstack ( n -- a )
  \
  \ Create an `xstack` in data space. _n_ is the size in
  \ cells.  Return its address _a_.
  \
  \ See also: `allocate-xstack`.
  \
  \ }doc

( >x x@ xdrop x> xdup xpick )

[unneeded] >x ?( need xp

: >x ( x -- ) ( X: -- x ) cell xp +!  xp @ ! ; ?)

  \ doc{
  \
  \ >x ( x -- ) ( X: -- x )
  \
  \ Move _x_ from the data stack to the `xstack`.
  \
  \ See also: `x>`, `x@`.
  \
  \ }doc

[unneeded] x> ?( need x@ need xdrop

: x> ( -- x ) ( X: x -- ) x@ xdrop ; ?)

  \ doc{
  \
  \ x> ( -- x ) ( X: x -- )
  \
  \ Move _x_ from the current `xstack` to the data stack.
  \
  \ See also: `x>`, `x@`.
  \
  \ }doc

[unneeded] x@ ?( need xp

: x@ ( -- x ) ( X: x -- x ) xp @ @ ; ?)

  \ doc{
  \
  \ x@ ( -- x ) ( X: x -- x )
  \
  \ Copy _x_ from the current `xstack` to the data stack.
  \
  \ See also: `x>`, `>x`.
  \
  \ }doc

[unneeded] xdrop ?( need xp

: xdrop ( X: x -- ) [ cell negate ] literal xp +! ; ?)

  \ doc{
  \
  \ xdrop ( X: x -- )
  \
  \ Remove _x_ from the `xstack`.
  \
  \ See also: `>x`, `x>`.
  \
  \ }doc

[unneeded] xdup ?( need x@ need >x

: xdup ( X: x -- x x ) x@ >x ; ?)

  \ doc{
  \
  \ xdup ( X: x -- x x )
  \
  \ Duplicate _x_ in the current `xstack`.
  \
  \ See also: `2xdup`.
  \
  \ }doc

[unneeded] xpick ?( need xp

: xpick ( u -- x'u ) ( X: x'u ... x'0 -- x'u ... x'0 )
  xp @ swap cells - @ ; ?)

  \ doc{
  \
  \ xpick ( u -- x'u ) ( X: x'u ... x'0 -- x'u ... x'0 )
  \
  \ Remove _u_. Copy _x'u_ from the current `xstack` to the
  \ data stack.
  \
  \ }doc

[unneeded] xover ?( need xpick need >x

: xover ( X: x1 x2 -- x1 x2 x1 ) 1 xpick >x ; ?)

  \ doc{
  \
  \ xover ( X: x1 x2 -- x1 x2 x1 )
  \
  \ Place a copy of _x1_ on top of the `xstack`.
  \
  \ }doc


( 2x@ 2>x 2x> 2xdrop 2xdup )

[unneeded] 2x@ ?( need x@ need xpick

: 2x@ ( -- x1 x2 ) ( X: x1 x2 -- x1 x2 )
  x@ 1 xpick swap ; ?)

  \ doc{
  \
  \ 2x@ ( -- x1 x2 ) ( X: x1 x2 -- x1 x2 )
  \
  \ Copy the cell pair _x1 x2_ from the current `xstack` to the
  \ data stack.
  \
  \ }doc

[unneeded] 2>x ?( need >x

: 2>x ( x1 x2 -- ) ( X: -- x1 x2 ) swap >x >x ; ?)

  \ doc{
  \
  \ 2>x ( x1 x2 -- ) ( X: -- x1 x2 )
  \
  \ Move the cell pair _x1 x2_ from the data stack to the
  \ current `xstack`.
  \
  \ See also: `2x>`, `2x@`, `>x`.
  \
  \ }doc

[unneeded] 2x> ?( need x>

: 2x> ( -- x1 x2 ) ( X: x1 x2 -- ) x> x> swap ; ?)

  \ doc{
  \
  \ 2x> ( -- x1 x2 ) ( X: x1 x2 -- )
  \
  \ Move the cell pair _x1 x2_ from the current `xstack` to
  \ the data stack.
  \
  \ See also: `2>x`, `2x@`, `x>`.
  \
  \ }doc

[unneeded] 2xdrop ?( need xp

: 2xdrop ( X: x1 x2 -- ) [ -2 cells ] literal xp +! ; ?)

  \ doc{
  \
  \ 2xdrop ( X: x1 x2 -- )
  \
  \ Remove the cell pair _x1 x2_ from the current `xstack`.
  \
  \ See also: `xdrop`.
  \
  \ }doc

[unneeded] 2xdup ?( need xover

: 2xdup ( X: x1 x2 -- x1 x2 x1 x2 ) xover xover ; ?)

  \ doc{
  \
  \ 2xdup ( X: x1 x2 -- x1 x2 x1 x2 ) xover xover ; ?)
  \
  \ Duplicate the cell pair _x1 x2_ in the current `xstack`.
  \
  \ See also: `xdup`.
  \
  \ }doc

( xclear xdepth .xs )

[unneeded] xclear ?( need xp0 need xp

: xclear ( -- ) xp0 xp ! ; ?)

  \ doc{
  \
  \ xclear ( -- )
  \
  \ Clear the current `xstack`.
  \
  \ See also: `xdrop`, `2xdrop`, `xp0`, `xp`.
  \
  \ }doc

[unneeded] xlen [unneeded] xdepth and ?( need xp need xp0

: xlen ( -- n ) xp @ xp0 - ;

  \ doc{
  \
  \ xlen ( -- n )
  \
  \ _n_ is the length of the current `xstack`, in address units.
  \
  \ See also: `xdepth`.
  \
  \ }doc

: xdepth ( -- n ) xlen cell / ; ?)

  \ doc{
  \
  \ xdepth ( -- n )
  \
  \ _n_ is the number of single-cells values contained in the
  \ current `xstack`.
  \
  \ See also: `.xs`, `xlen`.
  \
  \ }doc

[unneeded] .xs ?(

need xp0 need xlen need xdepth need .depth

: (.xs) ( -- ) xp0 cell+ xlen bounds ?do  i @ . cell +loop ;

  \ doc{
  \
  \ (.xs) ( -- )
  \
  \ Display a list of the items in the current `xstack`; TOS is
  \ the right-most item.
  \
  \ This word is a factor of `.xs`.
  \
  \ }doc

: .xs ( -- ) xdepth dup .depth if  (.xs)  then ; ?)

  \ doc{
  \
  \ .xs ( -- )
  \
  \ Display the number of items on the current `xstack`, followed
  \ by a list of the items, if any; TOS is the right-most item.
  \
  \ See also: `xdepth` ,`(.xs)`.
  \
  \ }doc

  \ vim: filetype=soloforth
