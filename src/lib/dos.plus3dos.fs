  \ dos.plus3dos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703081749

  \ -----------------------------------------------------------
  \ Description

  \ +3DOS support.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2017-03-04: Start. Move `/filename` ,`>filename`,
  \ `(rename-file)`, `rename-file`, `dos-get-1346` and
  \ `dos-set-1346` from the kernel.  Remove "dos-" prefix from
  \ `dos-get-1346` and `dos-set-1346`
  \
  \ 2017-03-08: Add `(delete-file` and `delete-file`. Improve
  \ documentation.

( /filename >filename (rename-file rename-file )

[unneeded] /filename ?\ 16 cconstant /filename

  \ doc{
  \
  \ /filename ( -- n )
  \
  \ Return the maximum length of a +3DOS filename.
  \
  \ See also: `>filename`.
  \
  \ }doc

[unneeded] >filename ?( need /filename

: >filename ( ca1 len1 -- ca2 )
  /filename min char+ save-string
  2dup + char- $FF swap c! drop ; ?)

  \ doc{
  \
  \ >filename  ( ca1 len1 -- ca2 )
  \
  \ Convert the filename _ca1 len1_ to a $FF-terminated string
  \ at _ca2_ in the `stringer`.
  \
  \ See also: `/filename`.
  \
  \ }doc

[unneeded] (rename-file ?(

code (rename-file ( ca1 ca2 -- ior )
  D1 c, E1 c, DD c, 21 c, 0127 , dos-ix-preserve-ip_ call,
  \ pop de
  \ pop hl
  \ ld ix,dos_rename
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ (rename-file  ( ca1 ca2 -- ior )
  \
  \ Rename filename _ca1_ (a $FF-terminated string) to filename
  \ _ca2_ (a $FF-terminated string) and return error result
  \ _ior_.
  \
  \ ``(rename-file`` is a factor of `rename-file`.
  \
  \ }doc

[unneeded] rename-file ?( need >filename need (rename-file

: rename-file ( ca1 len1 ca2 len2 -- ior )
  >filename >r >filename r> (rename-file ; ?)

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ rename-file  ( ca1 len1 ca2 len2 -- ior )
  \
  \ Rename the file named by the character string _ca1 len1_ to
  \ the name in the character string _ca2 len2_ and return
  \ error result _ior_.
  \
  \ Origin: Forth-94 (FILE EXT), Forth-2012 (FILE EXT).
  \
  \ See also: `(rename-file`.
  \
  \ }doc

( get-1346 set-1346 )

[unneeded] get-1346 ?(

code get-1346 ( -- n1 n2 n3 n4 )
  D9 c, DD c, 21 c, 013C , dos-ix_ call, 06 c, 00 c,
  \   exx                           ; save Forth IP
  \   ld ix,dos_get_1346
  \   call dos.ix
  \   ld b,0
  4A c, C5 c, 4B c, C5 c, 4C c, C5 c, 4D c, C5 c,
  \   ld c,d                         ; first sector buffer of cache
  \   push bc
  \   ld c,e                         ; number of cache sector buffers
  \   push bc
  \   ld c,h                         ; first sector buffer of RAM disk
  \   push bc
  \   ld c,l                         ; number of RAM disk sector buffers
  \   push bc
  D9 c, jpnext, end-code ?)
  \   exx                            ; restore Forth IP
  \   _jp_next

  \ doc{
  \
  \ get-1346 ( -- n1 n2 n3 n4 )
  \
  \ Return the +3DOS current configuration of RAM banks 1, 3, 4
  \ and 6, which are organized as an array of 128 sector
  \ buffers, each of 512 bytes. The cache and RAM disk occupy
  \ two separate (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See also: `set-1346`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

[unneeded] set-1346 ?(

code set-1346 ( n1 n2 n3 n4 -- )
  D9 c, E1 c, C1 c, 61 c, D1 c, C1 c, 51 c, DD c, 21 c, 013F ,
  \ exx                  ; save Forth IP
  \ pop hl               ; L = number of RAM disk sector buffers
  \ pop bc
  \ ld h,c               ; H = first buffer of RAM disk
  \ pop de               ; E = number of cache sector buffers
  \ pop bc
  \ ld d,c               ; D = first buffer of cache
  \ ld ix,dos_set_1346
  dos-ix_ call, D9 c, jpnext, end-code ?)
  \ call dos.ix
  \ exx                  ; restore Forth IP
  \ _jp_next

  \ doc{
  \
  \ set-1346 ( n1 n2 n3 n4 -- )
  \
  \ Set the +3DOS configuration of RAM banks 1, 3, 4 and 6,
  \ which are organized as an array of 128 sector buffers, each
  \ of 512 bytes. The cache and RAM disk occupy two separate
  \ (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See also: `dos-get-1346`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

( (delete-file delete-file )

[unneeded] (delete-file ?(

code (delete-file ( ca -- ior )
  E1 c, DD c, 21 c, 0124 , dos-ix-preserve-ip_ call,
  \ pop hl
  \ ld ix,dos_delete
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ (delete-file  ( ca -- ior )
  \
  \ Delete the disk file named in the $FF-terminated string
  \ _ca_ and return an error result _ior_.
  \
  \ ``(delete-file`` is a factor of `delete-file`.
  \
  \ }doc

[unneeded] delete-file ?( need >filename need (delete-file

: delete-file ( ca len -- ior ) >filename (delete-file ; ?)

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ delete-file ( ca len -- ior )
  \
  \ Delete the disk file named in the string _ca len_ and
  \ return an error result _ior_.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `(delete-file`.
  \
  \ }doc

  \ vim: filetype=soloforth
