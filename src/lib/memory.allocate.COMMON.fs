  \ memory.allocate.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The code common to both implementations of `allocate`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( heap /heap heap-bank heap-in heap-out allocate resize free )

need value

0 value heap

  \ doc{
  \
  \ heap  ( -- a )
  \
  \ Address of the current memory heap, used by `allocate`,
  \ `resize` and `free`.
  \
  \ The memory heap can be created by `allot-heap`,
  \ `limit-heap`, `bank-heap`, or `farlimit-heap`. Then it must
  \ be initialized by `empty-heap`.
  \
  \ See also: `/heap`, `get-heap`.
  \
  \ }doc

0 value /heap

  \ doc{
  \
  \ /heap  ( -- n ) "slash-heap"
  \
  \ Size of the current `heap`, in bytes.
  \
  \ See also: `get-heap`.
  \
  \ }doc

create heap-bank ( -- ca ) 0 c,

  \ doc{
  \
  \ heap-bank  ( -- ca )
  \
  \ A `cvariable` _ca_ that contains the memory bank
  \ used to store the `heap`, when the memory heap was created
  \ by `bank-heap` or `farlimit-heap`. If the heap was created
  \ by `allot-heap` or `limit-heap`, ``heap-bank`` contains
  \ zero.
  \
  \ See also: `heap-in`, `heap-out`, `get-heap`.
  \
  \ }doc

defer heap-in ( -- ) ' noop ' heap-in defer!

  \ doc{
  \
  \ heap-in  ( -- )
  \
  \ If the current `heap` was created by `bank-heap` or
  \ `farlimit-heap`, page in its bank, which is stored at
  \ `heap-bank`; else do nothing.
  \
  \ ``heap-in`` is a deferred word (see `defer`) whose default
  \ action is `noop`. Its alternative action is `(heap-in`.
  \
  \ See also: `heap-out`.
  \
  \ }doc

defer heap-out ( -- ) ' noop ' heap-out defer!

  \ doc{
  \
  \ heap-out  ( -- )
  \
  \ If the current `heap` was created by `bank-heap` or
  \ `farlimit-heap`, page in the default memory bank instead;
  \ else do nothing.
  \
  \ ``heap-out`` is a deferred word (see `defer`) whose default
  \ action is `noop`. Its alternative action is `default-bank`.
  \
  \ See also: `heap-in`.
  \
  \ }doc

defer allocate ( u -- a ior )

  \ doc{
  \
  \ allocate ( u -- a ior )
  \
  \ Allocate _u_ bytes of contiguous data space. The data-space
  \ pointer is unaffected by this operation. The initial
  \ content of the allocated space is undefined.
  \
  \ If the allocation succeeds, _a_ is the starting address of
  \ the allocated space and _ior_ is zero.
  \
  \ If the operation fails, _a_ does not represent a valid
  \ address and _ior_ is the I/O result code.
  \
  \ ``allocate`` is a deferred word (see `defer`) whose action
  \ can be `charlton-allocate` or `gil-allocate`, depending on
  \ the `heap` implementation used by the application.
  \
  \ Origin: Forth-94 (MEMORY), Forth-2012 (MEMORY).
  \
  \ See also: `free`, `resize`, `empty-heap`.
  \
  \ }doc

defer resize ( a1 -- a2 ior )

  \ doc{
  \
  \ resize ( a1 -- a2 ior )
  \
  \ Change the allocation of the contiguous data space starting
  \ at the address _a1_, previously allocated  by `allocate` or
  \ ``resize``, to _u_ bytes. _u_ may be either larger or
  \ smaller than the current size of the region. The data-space
  \ pointer is unaffected by this operation.
  \
  \ If the operation succeeds, _a2_ is  the starting address of
  \ _u_ bytes of allocated  memory and _ior_ is zero.  _a2_ may
  \ be,  but need not be,  the same as _a1_. If they are  not
  \ the same,  the values contained in the region at _a1_ are
  \ copied to _a2_, up to the minimum size of either of  the
  \ two regions. If they are the same, the values contained in
  \ the region are preserved to the minimum of _u_ or the
  \ original size. If _a2_ is not the same as _a1_, the region
  \ of memory at _a1_ is returned to the system according to
  \ the operation of `free`.
  \
  \ If the operation fails, _a2_ equals _a1_, the region of
  \ memory at _a1_ is unaffected, and  ior is the I/O result
  \ code.
  \
  \ ``resize`` is a deferred word (see `defer`) whose action
  \ can be `charlton-resize`, depending on the `heap`
  \ implementation used by the application.
  \
  \ Origin: Forth-94 (MEMORY), Forth-2012 (MEMORY).
  \
  \ See also: `allocate`, `free`, `empty-heap`.
  \
  \ }doc

defer free ( a -- ior )

  \ doc{
  \
  \ free ( a -- ior )
  \
  \ Return the contiguous region of data space indicated by _a_
  \ to the system for later allocation. _a_ shall indicate a
  \ region of data space that was previously obtained by
  \ `allocate` or `resize`.
  \
  \ If the operation succeeds, _ior_ is zero. If the operation
  \ fails, _ior_ is the I/O result code.
  \
  \ ``free`` is a deferred word (see `defer`) whose action can
  \ be `charlton-free` or `gil-free`, depending on the `heap`
  \ implementation used by the application.
  \
  \ Origin: Forth-94 (MEMORY), Forth-2012 (MEMORY).
  \
  \ See also: `allocate`, `resize`, `empty-heap`.
  \
  \ }doc

defer empty-heap ( -- )

  \ doc{
  \
  \ empty-heap ( -- )
  \
  \ Empty the current `heap`, which was created by
  \ `allot-heap`, `limit-heap`, `bank-heap` or `farlimit-heap`.
  \
  \ ``empty-heap`` is a deferred word (see `defer`) whose
  \ action can be `charlton-empty-heap` or `gil-empty-heap`,
  \ depending on the `heap` implementation used by the
  \ application.
  \
  \ }doc

( allot-heap limit-heap farlimit-heap bank-heap )

unneeding allot-heap ?( need /heap need heap

: allot-heap ( n -- a )
  dup to /heap here to heap allot heap ; ?)

  \ doc{
  \
  \ allot-heap ( n -- a )
  \
  \ Create a `heap` of _n_ bytes in the data space.  Return its
  \ address _a_.
  \
  \ See also: `limit-heap`, `bank-heap`, `farlimit-heap`,
  \ `empty-heap`.
  \
  \ }doc

unneeding limit-heap ?( need /heap need heap

: limit-heap ( n -- a )
  dup to /heap negate limit +! limit @ dup to heap ; ?)

  \ doc{
  \
  \ limit-heap ( n -- a )
  \
  \ Create a `heap` of _n_ bytes right above `limit` and return
  \ its address _a_. `limit` is moved down _n_ bytes.
  \
  \ See also: `allot-heap`, `bank-heap`, `farlimit-heap`,
  \ `empty-heap`.
  \
  \ }doc

unneeding farlimit-heap ?( need /heap need heap

: farlimit-heap ( n -- a )
  dup to /heap negate farlimit +!  farlimit @ dup to heap
  dup far bank-index c@ far-banks + c@ heap-bank c! ;

  \ doc{
  \
  \ farlimit-heap ( n -- a )
  \
  \ Create a `heap` of _n_ bytes right above `farlimit` and
  \ return its address _a_. `farlimit` is moved down _n_ bytes,
  \ and `heap-bank` is updated with the corresponding bank.
  \
  \ `allocate`, `resize` and `free` page in the corresponding
  \ bank at the start and restore the default bank at the end.
  \
  \ WARNING: The heap must be in one memory bank.  Therefore,
  \ before executing ``farlimit-heap``, the application must
  \ check that the _n_ bytes below `farlimit` belong to one
  \ memory bank.
  \
  \ See also: `allot-heap`, `bank-heap`, `limit-heap`, `empty-heap`.
  \
  \ }doc

unneeding bank-heap ?( need alias

: bank-heap ( a n b -- a ) heap-bank c! to /heap to heap ;

  \ doc{
  \
  \ bank-heap ( n b a -- a )
  \
  \ Create a `heap` of _n_ bytes at address _a_ of bank _b_.
  \ _a_ is the actual address ($C000..$FFFF) when bank _b_ is
  \ paged in, which is stored in `heap-bank`.
  \
  \ `allocate`, `resize` and `free` page in bank _b_ at the
  \ start and restore the default bank at the end.
  \
  \ See also: `heap-in`, `heap-out`, `allot-heap`, `limit-heap`,
  \ `farlimit-heap`, `empty-heap`.
  \
  \ }doc

: (heap-in  ( -- ) heap-bank c@ ?dup 0exit bank ;

  \ doc{
  \
  \ (heap-in  ( -- ) "paren-heap-in"
  \
  \ If the current `heap` was created by `bank-heap`, page in
  \ its bank, which is stored at `heap-bank`; else do nothing.
  \
  \ ``(heap-in`` is the action of `heap-in`.
  \
  \ }doc

' (heap-in ' heap-in defer! ' default-bank ' heap-out defer! ?)

( get-heap set-heap )

unneeding get-heap
?\ : get-heap ( -- a u b ) heap /heap heap-bank c@ ;

  \ doc{
  \
  \ get-heap ( -- a u b )
  \
  \ Get the values of the current heap: its address _a_
  \ (returned by `heap`), its size _u_ (returned by `/heap`)
  \ and its bank _b_ (stored in `heap-bank`).
  \
  \ ``get-heap`` and `set-heap` are useful when more than one
  \ memory heap are needed by the application.
  \
  \ }doc

unneeding set-heap
?\ : set-heap ( a u b -- ) heap-bank c! to /heap to heap ;

  \ doc{
  \
  \ set-heap ( a u b -- )
  \
  \ Set the values of the current heap: its address _a_
  \ (returned by `heap`), its size _u_ (returned by `/heap`)
  \ and its bank _b_ (stored in `heap-bank`).
  \
  \ ``set-heap`` and `get-heap` are useful when more than one
  \ memory heap are needed by the application.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-30: Start. Add `heap`, `/heap`, `allot-heap`,
  \ `bank-heap`, `heap-bank`, `heap-in`, `heap-out`,
  \ `limit-heap`...
  \
  \ 2017-04-09: Add `farlimit-heap`. Improve documentation. Add
  \ `get-heap` and `set-heap`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-04-16: Improve description of _ior_ notation.
  \
  \ 2018-06-04: Update documentation: remove mentions of
  \ aligned addresses.
  \
  \ 2020-06-08: Update: now `0exit` is in the kernel.
  \
  \ 2020-06-15: Improve documentation: Add cross-reference to
  \ `cvariable`.
  \
  \ 2020-07-28: Improve documentation of deferred words.

  \ vim: filetype=soloforth
