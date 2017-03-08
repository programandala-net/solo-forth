  \ dos.plus3dos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703082342

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
  \ documentation. Add `r/o`, `w/o`, `r/w`, `bin`, `s/r`,
  \ `do-dos-open_`, `file-id-table`, `file-id`, and drafts of
  \ `create-file` and `open-file`. Move `close-file` from the
  \ kernel and improve it to update `file-id-table`.

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
  \ >filename ( ca1 len1 -- ca2 )
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
  \ (rename-file ( ca1 ca2 -- ior )
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
  \ rename-file ( ca1 len1 ca2 len2 -- ior )
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
  \ (delete-file ( ca -- ior )
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

( max-file-id file-id-table file-id )

15 cconstant max-file-id

create file-id-table max-file-id 1+ allot
file-id-table max-file-id 1+ erase
  \ XXX TODO rename?
  \ XXX TODO -- use also to remember which files are headed, one bit

: file-id ( -- fid true | false )
  max-file-id 1+ 0 ?do
    i file-id-table + dup c@ 0=
    if $FF swap c! i true unloop exit then drop
  loop false ;

( r/o w/o r/w s/r bin headed do-dos-open_ )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

[unneeded] r/o ?\ %001 cconstant r/o
[unneeded] w/o ?\ %010 cconstant w/o
[unneeded] r/w ?\ %011 cconstant r/w
[unneeded] s/r ?\ %101 cconstant s/r
[unneeded] bin ?\ need alias ' noop alias bin immediate

[unneeded] headed ?\ : headed ( fam1 -- fam2 ) 128 and ;
  \ XXX TODO -- Rewrite in Z80.
  \ pop hl
  \ set 7,l
  \ jp pushl

[unneeded] do-dos-open_ ?( need assembler

create do-dos-open_ ( -- a ) asm

  \ This oced is used by '(create-file' and `(open-file`
  \
  \ Entry conditions:
  \   B = fid
  \   C = fam
  \   D = create action
  \   E = open action
  \   HL = address of filename (no wildcards), with trailing 0xFF
  \   (TOS) = Forth IP
  \   (NOS) = fid + 256*fam

  0106 ix ldp#, dos-ix_ call,
  \   ld ix,dos_open
  \   call dos.ix
  b pop, nc? pushdosior ?jp,
  h pop, 0 h ld#, h push, ' false jp, end-asm ?)
  \   pop bc  ; restore the Forth IP
  \   _jump_nc do_dos_open.error
  \   ; no error
  \   pop hl      ; l = fid
  \   ld h,0
  \   push hl     ; fid
  \   jp false_   ; no error
  \
  \ do_dos_open.error
  \   ; (sp) = fid + 256*fam (used as undefined fid)
  \   ; a = error code

  \   call convert_dos_error_code
  \   ; hl = error code
  \   jp push_hl

( create-file )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

need assembler need >filename need file-id need do-dos-open_

code (create-file ( ca fam fid -- fid ior )
  d pop, h pop, l d ld, h pop, d push, b push, e b ld, d c ld,
  \   pop de  ; E = fid
  \   pop hl  ; L = fam
  \   ld d,l  ; D = fam
  \   pop hl  ; HL = ca
  \   push de ; save fid
  \   push bc ; save the Forth IP
  \   ld b,e  ; B = fid
  \   ld c,d  ; C = fam
  \   ; Calculate the create action
  c 7 bit, c 7 res, 1 a ld#, z? rif a inc, rthen
  \   bit 7,c ; headed? (maybe set by 'headed')
  \   res 7,c
  \   ld a,1  ; create action 1: new file with header; position after the header
  \   jr nz,paren_open_file_.actions ; jump if headed
  \   ; Create a file with no header
  \   inc a   ; create action 2: new file without a header, position at 0
  \ paren_open_file_.actions:
  \   ; A = open/create action
  a d ld, a e ld, do-dos-open_ jp, end-code
  \   ld d,a  ; D = create action
  \   ld e,a  ; E = open action
  \   jp do_dos_open

  \ ****************************
  \
  \ XXX FIXME
  \
  \ When 'headed' is used, 'create-file'
  \ fails with DOS error 23 (file not
  \ found), no matter the fam, e.g.:
  \
  \   s" headed" w/o headed create-file
  \
  \ It works fine without 'headed', or when
  \ the file already exists.
  \
  \ But 'open-file' works fine with and
  \ without 'header', and its code is
  \ identical except the actions.
  \
  \ Tried with ZX Spectrum +3e too, no
  \ difference.

: create-file ( ca len fam -- fid ior )
  >r >filename r> file-id if (create-file exit then
                          drop #-288 ;

( open-file )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

need assembler need >filename need file-id need do-dos-open_

code (open-file ( ca fam fid -- fid ior )
  d pop, h pop, l d ld, h pop, d push, b push, e b ld, d c ld,
  \   pop de  ; E = fid
  \   pop hl  ; L = fam
  \   ld d,l  ; D = fam
  \   pop hl  ; HL = ca
  \   push de ; save fid
  \   push bc ; save the Forth IP
  \   ld b,e  ; B = fid
  \   ld c,d  ; C = fam
  \   ; Calculate the open action
  c 7 bit, c 7 res, 1 a ld#, z? rif a inc, rthen
  \   bit 7,c ; headed? (maybe set by 'headed')
  \   res 7,c
  \   ld a,1  ; open action 1: position after the header
  \   jr nz,paren_open_file_.actions ; jump if headed
  \   ; Open a file with no header
  \   inc a   ; open action 2: ignore any header, position at 0
  \ paren_open_file_.actions:
  \   ; A = open action
  0 d ld#, a e ld, do-dos-open_ jp, end-code
  \   ld d,0  ; D = create action: error, file does not exist
  \   ld e,a  ; E = open action
  \   jp do_dos_open

: open-file ( ca len fam -- fid ior )
  >r >filename r> file-id if (open-file exit then drop #-288 ;

( close-file )

code (close-file ( fid -- ior )
  E1 c, C5 c, 45 c, dd c, 21 c, 0109 , dos-ix_ call, C1 c,
  \ pop hl          ; L = file identifier
  \ push bc         ; save Forth IP
  \ ld b,l          ; B = file identifier
  \ ld ix,dos_close
  \ call dos.ix
  \ pop bc          ; restore Forth IP
  pushdosior jp, end-code
  \ jp push_dos_ior

  \ doc{
  \
  \ (close-file ( fid -- ior )
  \
  \ Close the file identified by _fid_ and return error result
  \ _ior_.
  \
  \ ``(close-file`` is a factor of `(close-file`.
  \ ``(close-file`` closes the file, but does not update
  \ `file-id-table`.
  \
  \ }doc

: close-file ( fid -- ior )
  dup >r (close-file dup 0<> file-id-table r> + c! ;

  \ doc{
  \
  \ close-file ( fid -- ior )
  \
  \ Close the file identified by _fid_ and return error result
  \ _ior_.
  \
  \ See also: `(close-file`.
  \
  \ }doc

  \ vim: filetype=soloforth
