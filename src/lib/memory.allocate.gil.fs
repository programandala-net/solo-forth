  \ memory.allocate.gil.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ An alternative implementation of the
  \ common heap based on code written by Javier Gil.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ Credit

  \ Based on "Gestor de memoria dinámica (version 1)" by Javier
  \ Gil, from his book _Introducción a Forth_ (2007-01),
  \ <http://disc.ua.es/~gil/#forth>.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-11-18: Start.
  \
  \ 2015-11-21: Changes.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

( create-heap )

need value need set-bit need reset-bit need bit?
need reserve need alias

0 value heap ( -- a )
  \ Address of the current heap.

8 constant address-unit-bits
16 value /chunk \ bytes per chunk

: groups ( n1 n2 -- n3 ) /mod swap 0<> abs + ;
  \ Return the number _n3_ of groups of _n2_ elements, needed
  \ to hold _n1_ elements.

: bytes>chunks ( n1 -- n2 ) /chunk groups ;
  \ Return the chunks _n2_ required to allocate _n1_ bytes.

: chunks>bytes ( n1 -- n2 ) address-unit-bits groups ;
  \ Return the bytes _n2_ required for a bitmap of _n1_ chunks.

' heap alias heap-chunks ( -- a )
  \ Address that holds the number of chunks of the current
  \ heap.

: heap-unused-chunks ( -- a ) heap-chunks cell+ ;
  \ Address that holds the number of unused chunks of the
  \ current heap.

: heap-map ( -- a ) heap-unused-chunks cell+ ;
  \ Address of the current heap's map.

: /heap-map ( -- n ) heap-chunks @ chunks>bytes ;
  \ Number of bytes of the current heap's map.

: heap-data ( -- a ) heap-map /heap-map + ;  -->
  \ Address of the current heap's data space.

( create-heap )

: (mapbit) ( n1 -- n2 ca )
  address-unit-bits /mod heap-map + ;
  \ n1 = number of bit in the bitmap
  \ n2 = number of bit in the byte at _a2_
  \ ca = address of the bitmap that holds bit _n2_

: mapbit ( n1 -- a2 b n2 ) (mapbit) dup @ rot ;
  \ n1 = number of bit in the bitmap
  \ a2 = address of the correspondent byte
  \ b =  correspondent byte
  \ n2 = number of bit in _b_

: used-chunk? ( n -- f ) (mapbit) @ swap bit? ;
  \ Is chunk _n_ used?

: use-chunk ( n -- ) mapbit set-bit swap ! ;
  \ Mark chunk _n_ as used.

: free-chunk ( n -- ) mapbit reset-bit swap ! ;
  \ Mark chunk _n_ as free.

: allocated>chunks ( a -- n ) cell- @ ;
  \ Convert the address _a_ of an allocated space in the
  \ current heap to its number of chunks.

: allocated>index ( a -- n ) heap-data - /chunk / ;
  \ Convert the address _a_ of an allocated space in the
  \ current heap to its index _n_ in the map of the heap
  \ chunks.

: allocated>map ( a -- n1 n2 )
  dup allocated>index swap allocated>chunks ;
  \ Convert the address _a_ of an allocated space in the
  \ current heap to its index _n1_ in the map of heap chunks
  \ and the number _n2_ of ocuppied chunks.

-->

( create-heap )

: locate-chunks ( n1 -- n1 n2 0 | ior )
  0 tuck ( n2 n1 count )
  heap-chunks @ 0 ?do ( n2 n1 count )
    i used-chunk? if  drop >r i 1+ r> 0  else  1+  then
    2dup = if  drop swap 0 unloop exit  then
  loop  2drop drop -59 ;
  \ Locate _n1_ consecutive free chunks in the current heap.
  \ If succesful, _n2_ is the first chunk of the group;
  \ else return _ior_ -59, the error code for `allocate`.

: chunk>address ( n1 -- a ) /chunk * heap-data + ;

: (allocate) ( n1 n2 -- a )
  dup chunk>address >r
  swap bounds ?do  i use-chunk  loop
  r> ;
  \ Allocate _n1_ chunks of the current heap, starting from
  \ chunk _n2_; return the address _a_ of the allocated space.

-->

( create-heap )

  \ User interface

: create-heap ( n "name" -- )
  create  bytes>chunks dup ,  0 ,
            \ max chunks and free chunks
          dup chunks>bytes reserve drop
            \ bitmap
          /chunk * allot ;
            \ data space
  \ Create a new heap "name" to hold _n_ bytes.

: allocate ( n -- a ior )
  bytes>chunks locate-chunks ?dup ?exit  (allocate) 0 ;

: free ( a -- ior )
  allocated>map dup >r
  bounds ?do  i free-chunk  loop
  r> heap-unused-chunks +! 0 ;

  \ XXX TODO -- update with `resize`
  \ doc{
  \
  \ free ( a -- ior )
  \
  \ Return the contiguous region of data space indicated by _a_
  \ to the system for later allocation. _a_ shall indicate a
  \ region of data space that was previously obtained by
  \ `allocate`.
  \
  \ If the operation succeeds, _ior_ is zero. If the operation
  \ fails, _ior_ is -60.
  \
  \ Origin: Forth-94 (MEMORY), Forth-2012 (MEMORY).
  \
  \ }doc

: empty-heap ( -- ) heap-chunks @ 0 ?do  i free-chunk  loop ;
  \ Empty the current heap, setting all chunks free.

: .heap ( -- )
  heap-chunks @ 0 ?do
    i used-chunk? if  'x'  else  '-'  then  emit
  loop ;

  \ Print the map of the current heap. Occupied chunks are
  \ marked with a "x"; free chunks are marked with a "-".

  \ vim: filetype=soloforth

