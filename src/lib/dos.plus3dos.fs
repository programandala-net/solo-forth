  \ dos.plus3dos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709090946
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ +3DOS support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /filename /base-filename /filename-ext >filename )

[unneeded] /filename ?\ 16 cconstant /filename

  \ doc{
  \
  \ /filename ( -- n )
  \
  \ Return the maximum length of a +3DOS filename, including
  \ drive, user area and filename extension.
  \
  \ See also: `/base-filename`, `>filename`.
  \
  \ }doc

[unneeded] /base-filename ?\ 8 cconstant /base-filename

  \ doc{
  \
  \ /base-filename ( -- n )
  \
  \ Return the maximum length of a +3DOS base filename, i.e.,
  \ a filename without drive, user area and extension.
  \
  \ See also: `/filename-ext`, `/filename`, `>filename`.
  \
  \ }doc

[unneeded] /filename-ext ?\ 3 cconstant /filename-ext

  \ doc{
  \
  \ /filename-ext ( -- n )
  \
  \ Return the maximum length of a +3DOS filename extension
  \ excluding the dot.
  \
  \ See also: `/filename`, `/base-filename`.
  \
  \ }doc


[unneeded] >filename ?( need /filename

: >filename ( ca1 len1 -- ca2 )
  /filename min char+ >stringer
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

( (rename-file rename-file )

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

( file-position reposition-file )


[unneeded] file-position ?(

code file-position ( fid -- ud ior )
  E1 c, C5 c, 45 c, DD c, 21 c, 0139 , dos-ix_ call,
  \   pop hl ; L = fid
  \   push bc ; save Forth IP
  \   ld b,l ; fid
  \   ld ix,dos_get_eof
  \   call dos.ix
  C1 c, E5 c, D5 c, pushdosior jp, end-code ?)
  \   pop bc ; restore Forth IP
  \   push hl ; low part of _ud_
  \   push de ; high part of _ud_
  \   jp push_dos_ior

  \ doc{
  \
  \ file-position ( fid -- ud ior )
  \
  \ Return the the current file position _ud_ for the file
  \ identified by _fid_, and error result _ior_. If _ior_ is
  \ non-zero, _ud_ is undefined.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `reposition-file`, `open-file`, `create-file`.
  \
  \ }doc

[unneeded] reposition-file ?(

code reposition-file ( ud fid -- ior )
  E1 c, 7D c, D1 c, E1 c, C5 c, 47 c, DD c, 21 c, 0136 ,
  \ pop hl
  \ ld a,l ; A = fid
  \ pop de ;
  \ pop hl ; EHL = file pointer
  \ push bc ; save Forth IP
  \ ld b,a ; B = fid
  \ ld ix,dos_set_position
  dos-ix_ call, C1 c, pushdosior jp, ?)
  \ call dos.ix
  \ pop bc ; restore Forth IP
  \ jp push_dos_ior

  \ doc{
  \
  \ reposition-file ( ud fid -- ior )
  \
  \ Reposition the file identified by _fid_ to _ud_ and return
  \ error result _ior_.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `file-position`, `open-file`, `create-file`.
  \
  \ }doc

( (cat )

  \ cr .( loading (cat ... ) \ XXX INFORMER

need assembler need >filename
need /base-filename need /filename-ext

13 cconstant /cat-entry

  \ doc{
  \
  \ /cat-entry ( -- n )
  \
  \ Return size _n_, in bytes, of every entry of the
  \ `cat-buffer` used by `cat`.
  \
  \ See also: `cat-entries`.
  \
  \ }doc

create cat-entries 10 c,
  \ XXX TODO -- Convert to constant. Or move the buffer to
  \ `pad` and calculate its length.

  \ doc{
  \
  \ cat-entries ( -- ca )
  \
  \ A character variable that holds the number of entries
  \ (minimum 2) of the `cat-buffer`, and used by `(cat`.  Its
  \ default value is 10.
  \
  \ See also: `/cat-entry`.
  \
  \ }doc

create cat-buffer cat-entries c@ 1+ /cat-entry * allot

  \ doc{
  \
  \ cat-buffer ( -- a )
  \
  \ _a_ is the address of the catalogue buffer, used by `(cat`,
  \ `.cat`, `.acat` and other words.
  \
  \ See also: `cat-entries`, `/cat-entry`.
  \
  \ }doc


: .filename-ext ( ca -- ) '.' emit /filename-ext type ;

  \ doc{
  \
  \ .filename-ext ( -- ca )
  \
  \ Display the filename extension whose `/filename-ext`
  \ characters (left justified, space filled) are stored at
  \ _ca_. A dot separator is printed first, which is not
  \ included in the string at _ca_.
  \
  \ See also: `.filename`.
  \
  \ }doc

: .filename ( ca -- )
  /base-filename 2dup type + .filename-ext ; -->

  \ doc{
  \
  \ .filename ( ca -- )
  \
  \ Display the filename whose characters are stored at _ca_,
  \ in two parts: `/base-filename` characters (left justified,
  \ space filled) followed by `/filename-ext` characters (left
  \ justified, space filled).  The dot that separates the base
  \ filename from the filename extension is not included in the
  \ string at _ca_, but it's printed.
  \
  \ See also: `.filename-ext`.
  \
  \ }doc

( (cat )

: >cat-entry ( n -- ca ) /cat-entry * cat-buffer + ;

  \ doc{
  \
  \ >cat-entry ( n -- ca )
  \
  \ Convert `cat-buffer` entry _n_ to its address _ca_.
  \
  \ See also: `/cat-entry`.
  \
  \ }doc

variable full-cat  full-cat on

  \ doc{
  \
  \ full-cat ( -- a )
  \
  \ _a_ is the address of a cell containing a flag. When the
  \ flag is _true_, `cat`, `wcat`, `acat` and `wacat` display
  \ also system files. When the flag is _false_, they don't.
  \ Other values are not supported.
  \ The default value is _true_.
  \
  \ See also: `>cat`.
  \
  \ }doc

: >cat ( ca len -- ca1 ca2 x )
  >filename cat-buffer dup /cat-entry erase
            cat-entries c@ 1+ $100 * full-cat @ abs or ;

  \ doc{
  \
  \ >cat ( ca len -- ca1 ca2 x )
  \
  \ Convert filename _ca len_ (wildcards permitted) to the
  \ parameters needed by `(cat`:
  \

  \ [horizontal]
  \ _ca1_ :: address of $FF-terminated filename (wildcards permitted)
  \ _ca2_ :: address of buffer
  \ _x_ (low byte) :: filter: bit 0 set if system files are
  \ included (configurable by `full-cat`)
  \ _x_ (high byte) :: size of the buffer in entries, plus one (>=2)

  \ See also: `wcat`, `cat`, `cat-entries`.
  \
  \ }doc

: more-cat ( -- )
  cat-entries c@ >cat-entry cat-buffer /cat-entry move ;

  \ doc{
  \
  \ more-cat ( -- )
  \
  \ Copy the last catalogue entry of `cat-buffer` to the first
  \ position in the buffer, in order to get the next chunk of
  \ the catalogue.
  \
  \ ``more-cat`` is a factor of `wcat`.
  \
  \ See also: `(cat`.
  \
  \ }doc

: more-cat? ( n -- f ) cat-entries c@ 1+ = ;

  \ doc{
  \
  \ more-cat? ( n -- f )
  \
  \ There may be more catalague entries to come after _n_ of
  \ them have been completed in `cat-buffer`?
  \
  \ ``more-cat?`` is a factor of `wcat` and `wacat`.
  \
  \ See also: `(cat`.
  \
  \ }doc

code (cat ( ca1 ca2 x -- n ior )
  exx, b pop, d pop, h pop,
       011E ix ldp#, dos-ix_ call, b c ld, 0 b ld#, b push,
  exx, pushdosior jp, end-code

  \ XXX TODO -- Rewrite with Z80 opcodes.

  \ doc{
  \
  \ (cat ( ca1 ca2 x -- n ior )
  \
  \ Fill a buffer _ca2_ with part of the directory (sorted),
  \ using filename stored at _ca1_. Input and output
  \ parameters:
  \

  \ [horizontal]
  \ _ca1_ :: address of $FF-terminated filename (wildcards permitted)
  \ _ca2_ :: address of buffer
  \ _x_ (low byte) :: bit 0 set if system files are included
  \ _x_ (high byte) :: size of the buffer in entries, plus one (>=2)
  \ _n_ :: number of completed entries in buffer (if non-zero, there may be more to come)
  \ _ior_ :: result error (if non-zero, _n_ is undefined)

  \
  \ ``(cat`` is a factor of `wcat` and a direct interface to
  \ the DOS CATALOG +3DOS routine.
  \
  \ Entry 0 of the buffer must be preloaded with the first
  \ filename required (or erased with zeroes). Entry 1 will
  \ contain the first matching filename greater than the
  \ preloaded entry (if any). If the buffer is too small for
  \ the catalogue, ``(cat`` can be called again with entry 0
  \ replaced by entry _n_ (task done by `more-cat`) to fetch
  \ the next part of the directory.
  \
  \ See also: `cat-buffer`, `cat-entries`, `/cat-entry`.
  \
  \ }doc

( wcat cat )

  \ cr .( loading cat ... ) \ XXX INFORMER

need (cat need 3dup need 3drop

: .cat-entry ( ca -- )
  dup .filename
      /base-filename /filename-ext + + @ 4 .r ."  KiB" ;

  \ doc{
  \
  \ .cat-entry ( ca -- )
  \
  \ Display a catalogue entry stored at _ca_. Format of the
  \ entry:
  \
  \ - Bytes 0..7: Base filename (left justified, space filled)
  \ - Bytes 8..10: Filename extension (left justified, space filled)
  \ - Bytes 11..12: File size in kibibytes (binary)
  \
  \ The file size is the amount of disk space allocated to the
  \ file, not necessarily the same as the amount used by the
  \ file.
  \
  \ ``.cat-entry`` is a factor of `.cat-entry#`.
  \
  \ See also: `.cat`, `.filename`.
  \
  \ }doc

: .cat ( n -- ) 1 ?do  i >cat-entry .cat-entry cr loop ;

  \ doc{
  \
  \ .cat ( n -- )
  \
  \ Display _n_ entries from `cat-buffer`.
  \
  \ ``.cat`` is a factor of `wcat`.
  \
  \ See also: `.cat-entry`, `.acat`.
  \
  \ }doc

: wcat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                          while  dup .cat more-cat?
                          while  more-cat
                          repeat then 3drop ;

  \ doc{
  \
  \ wcat ( ca len -- )
  \
  \ Show a wild-card disk catalogue using the wild-card
  \ filename _ca len_.
  \
  \ See also: `cat`, `wacat`, `(cat`, `more-cat`, `set-drive`.
  \
  \ }doc

: cat ( -- ) s" *.*" wcat ;

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See also: `wcat`, `acat`, `(cat`, `set-drive`.
  \
  \ }doc

( wacat acat )

  \ cr .( loading acat ... ) \ XXX INFORMER

need (cat need tab need 3dup need 3drop

: .acat ( n -- ) 1 ?do  i >cat-entry .filename tab loop ;

  \ doc{
  \
  \ .acat ( n -- )
  \
  \ Display _n_ entries from `cat-buffer`, in abbreviated
  \ format.
  \
  \ ``.acat`` is a factor of `wacat`.
  \
  \ See also: `.filename`, `.cat`.
  \
  \ }doc

: wacat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                           while  dup .acat more-cat?
                           while  more-cat
                           repeat then 3drop ;

  \ doc{
  \
  \ wacat ( ca len -- )
  \
  \ Show an abbreviated wild-card disk catalogue using the
  \ wild-card filename _ca len_.
  \
  \ See also: `acat`, `wcat`, `(cat`, `more-cat`, `set-drive`.
  \
  \ }doc

: acat ( -- ) s" *.*" wacat ;

  \ doc{
  \
  \ acat ( -- )
  \
  \ Show an abbreviated disk catalogue of the current drive.
  \
  \ See also: `wacat`, `cat`, `(cat`, `set-drive`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-09: Move `reposition-file` and `file-position` from
  \ the kernel. Document them. Improve `reposition-file`. Add
  \ `/base-filename`, `/filename-ext`. Add drafts of `cat` and
  \ `wcat`.
  \
  \ 2017-03-12: Update the names of `stringer` words.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-09-07: Fix file size in `.cat-entry`.  Fix `wcat`: it
  \ called `(cat` only once, not until the catalogue is
  \ completed. Fix and improve documentation. Add `acat` and
  \ `wacat`. Add the `full-cat` flag.
  \
  \ 2017-09-09: Move the catalogue buffer from the `stringer`
  \ to a permanent location. The reason of the catalogue
  \ corruption was the buffer was overwritten by other strings
  \ during the process. Remove `allocate-cat-buffer`. Fix
  \ documentation. Convert the `cat-buffer` variable to a
  \ constant.

  \ vim: filetype=soloforth
