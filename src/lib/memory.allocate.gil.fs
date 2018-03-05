  \ memory.allocate.gil.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ An alternative implementation of the memory heap based on
  \ code written by Javier Gil.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ Credit

  \ Based on "Gestor de memoria dinámica (versión 1)" by Javier
  \ Gil, from his book _Introducción a Forth_ (2007-01),
  \ <http://disc.ua.es/~gil/#forth>.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Rename the interface words. Make the standard
  \ words deferred and move them to its own module, to be
  \ shared by the other implementation.

( gil-heap-wordlist )

get-order get-current only forth definitions

need value need set-bit need reset-bit need bit?
need reserve need alias need address-unit-bits need heap

wordlist dup constant gil-heap-wordlist dup set-current >order

  \ doc{
  \
  \ gil-heap-wordlist ( -- wid )
  \
  \ _wid_ is the word-list identifier of the word list that
  \ holds the words the memory `heap` implementation adapted
  \ from code written by Javier Gil (2007-01).
  \
  \ ``need gil-heap-wordlist`` is used to load the memory heap
  \ implementation and configure `allocate`, `free` and
  \ `empty-heap` accordingly. This implementation of the memory
  \ heap does not provide `resize`.
  \
  \ An alternative, bigger implementation of the memory heap is
  \ provided by `charlton-heap-wordlist`.
  \
  \ The actual heap must be created with `allot-heap`,
  \ `limit-heap`, `farlimit-heap` or `bank-heap`, which are
  \ independent from the heap implemention.
  \
  \ }doc

16 value /chunk \ bytes per chunk

: groups ( n1 n2 -- n3 ) /mod swap 0<> abs + ;
  \ Return the number _n3_ of groups of _n2_ elements, needed
  \ to hold _n1_ elements.

: bytes>chunks ( n1 -- n2 ) /chunk groups ;
  \ Return the chunks _n2_ required to allocate _n1_ bytes.

: chunks>bytes ( n1 -- n2 ) address-unit-bits groups ;
  \ Return the bytes _n2_ required by a bitmap of _n1_ chunks.

' heap alias heap-chunks ( -- a )
  \ Address of a cell containing the number of chunks of the
  \ current heap.

: heap-unused-chunks ( -- a ) heap-chunks cell+ ;
  \ Address of a cell containing the number of unused chunks of
  \ the current heap.

: heap-map ( -- a ) heap-unused-chunks cell+ ;
  \ Address of the current heap's map.

: /heap-map ( -- n ) heap-chunks @ chunks>bytes ;
  \ Number of bytes of the current heap's map.

: heap-data ( -- a ) heap-map /heap-map + ;
  \ Address of the current heap's data space.

: (mapbit) ( n1 -- n2 ca ) address-unit-bits /mod heap-map + ;
  \ n1 = number of bit in the bitmap
  \ n2 = number of bit in the byte at _a2_
  \ ca = address of the bitmap that holds bit _n2_

: mapbit ( n1 -- a2 b n2 ) (mapbit) dup @ rot ; -->
  \ n1 = number of bit in the bitmap
  \ a2 = address of the corresponding byte
  \ b =  corresponding byte
  \ n2 = number of bit in _b_

( gil-heap-wordlist )

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

: locate-chunks ( n1 -- n1 n2 0 | ior )
  0 tuck ( n2 n1 count ) heap-chunks @ 0 ?do ( n2 n1 count )
    i used-chunk? if  drop >r i 1+ r> 0  else  1+  then
    2dup = if  drop swap 0 unloop exit  then
  loop  2drop drop #-59 ;
  \ Locate _n1_ consecutive free chunks in the current heap.
  \ If succesful, _n2_ is the first chunk of the group;
  \ else return _ior_ #-59, the error code for `allocate`.

: chunk>address ( n1 -- a ) /chunk * heap-data + ;

: (allocate) ( n1 n2 -- a ) dup chunk>address >r swap bounds
                            ?do i use-chunk loop r> ; -->
  \ Allocate _n1_ chunks of the current heap, starting from
  \ chunk _n2_; return the address _a_ of the allocated space.

( gil-heap-wordlist )

  \ User interface

: gil-empty-heap ( -- )
  heap-in heap /heap erase /heap bytes>chunks heap ! heap-out ;

  \ doc{
  \
  \ gil-empty-heap ( -- )
  \
  \ Empty the current `heap`, which was created by
  \ `allot-heap`, `limit-heap`, `bank-heap` or `farlimit-heap`.
  \
  \ ``gil-empty-heap`` is the action of `empty-heap` in the
  \ memory `heap` implementation based on code written by
  \ Javier Gil, whose words are defined in `gil-heap-wordlist`.
  \
  \ See: `gil-allocate`, `gil-free`.
  \
  \ }doc

: gil-allocate ( u -- u ior )
  heap-in bytes>chunks locate-chunks ?dup ?exit (allocate) 0
  heap-out ;

  \ doc{
  \
  \ gil-allocate ( u -- a ior )
  \
  \ Allocate _u_ address units of contiguous data space. The
  \ data-space pointer is unaffected by this operation. The
  \ initial content of the allocated space is undefined.
  \
  \ If the allocation succeeds, _a_ is the aligned starting
  \ address of the allocated space and _ior_ is zero.
  \
  \ If the operation fails, _a_ does not represent a valid
  \ address and _ior_ is #-59.
  \
  \ ``gil-allocate`` is the action of `allocate` in the `heap`
  \ implementation based on written code written by Javier Gil,
  \ whose words are defined in `gil-heap-wordlist`.
  \
  \ See: `gil-free`.
  \
  \ }doc

: gil-free ( a -- ior ) heap-in allocated>map dup >r
                        bounds ?do i free-chunk loop
                        r> heap-unused-chunks +! 0 heap-out ;

  \ doc{
  \
  \ gil-free ( a -- ior )
  \
  \ Return the contiguous region of data space indicated by _a_
  \ to the system for later allocation. _a_ shall indicate a
  \ region of data space that was previously obtained by
  \ `gil-allocate`.
  \
  \ ``gil-free`` is the action of `free` in the `heap`
  \ implementation based on written code written by Javier Gil,
  \ whose words are defined in `gil-heap-wordlist`.
  \
  \ }doc

forth-wordlist set-current

need empty-heap ' gil-empty-heap ' empty-heap defer!
need allocate   ' gil-allocate   ' allocate   defer!
need free       ' gil-free       ' free       defer!

set-current set-order

( .gil-heap )

unneeding .gil-heap ?(

need gil-heap-wordlist gil-heap-wordlist >order

: .gil-heap ( -- ) heap-in
  heap-chunks @ 0 ?do i used-chunk? if 'x' else '-' then emit
                  loop heap-out ;

  \ doc{
  \
  \ .gil-heap ( -- )
  \
  \ Print the map of the current memory `heap`, in the
  \ implementation based on code written by Javier Gil, whose
  \ words are defined in `gil-heap-wordlist`.
  \
  \ Occupied chunks are marked with a 'x'; free chunks are
  \ marked with a '-'.
  \
  \ }doc

previous ?)

  \ XXX OLD
  \ : create-heap ( n "name" -- )
  \   create  bytes>chunks dup , 0 ,
  \             \ max chunks and free chunks
  \           dup chunks>bytes reserve drop  /chunk * allot ;
  \             \ bitmap and data space
  \ Create a new heap _name_ to hold _n_ bytes.

  \ ===========================================================
  \ Change log

  \ 2015-11-18: Start.
  \
  \ 2015-11-21: Changes.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-30: Compact the code. Improve documentation.
  \
  \ 2017-04-09: Add the "gil-" prefix to the interface words,
  \ which are the actions associated to the standard words.
  \ Compact the code, saving one block.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth

