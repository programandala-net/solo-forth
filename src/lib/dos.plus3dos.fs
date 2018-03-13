  \ dos.plus3dos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803131706
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ +3DOS support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /filename /base-filename /filename-ext >filename )

unneeding /filename ?\ 16 cconstant /filename

  \ doc{
  \
  \ /filename ( -- n ) "slash-filename"
  \
  \ Return the maximum length of a +3DOS filename, including
  \ drive, user area and filename extension.
  \
  \ See: `/base-filename`, `>filename`.
  \
  \ }doc

unneeding /base-filename ?\ 8 cconstant /base-filename

  \ doc{
  \
  \ /base-filename ( -- n ) "slash-basefilename"
  \
  \ Return the maximum length of a +3DOS base filename, i.e.,
  \ a filename without drive, user area and extension.
  \
  \ See: `/filename-ext`, `/filename`, `>filename`.
  \
  \ }doc

unneeding /filename-ext ?\ 3 cconstant /filename-ext

  \ doc{
  \
  \ /filename-ext ( -- n ) "slash-filename-ext"
  \
  \ Return the maximum length of a +3DOS filename extension
  \ excluding the dot.
  \
  \ See: `/filename`, `/base-filename`.
  \
  \ }doc

unneeding >filename ?( need /filename

: >filename ( ca1 len1 -- ca2 )
  /filename min char+ >stringer
  2dup + char- $FF swap c! drop ; ?)

  \ doc{
  \
  \ >filename ( ca1 len1 -- ca2 ) "to-filename"
  \
  \ Convert the filename _ca1 len1_ to a $FF-terminated string
  \ at _ca2_ in the `stringer`.
  \
  \ See: `/filename`.
  \
  \ }doc

( (rename-file rename-file )

unneeding (rename-file ?(

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
  \ (rename-file ( ca1 ca2 -- ior ) "paren-rename-file"
  \
  \ Rename filename _ca1_ (a $FF-terminated string) to filename
  \ _ca2_ (a $FF-terminated string) and return error result
  \ _ior_.
  \
  \ ``(rename-file`` is a factor of `rename-file`.
  \
  \ }doc

unneeding rename-file ?( need >filename need (rename-file

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
  \ See: `(rename-file`.
  \
  \ }doc

( get-1346 set-1346 2-block-drives )

unneeding get-1346 ?(

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
  \ get-1346 ( -- n1 n2 n3 n4 ) "get-1-3-4-6"
  \
  \ Return the +3DOS current configuration of RAM banks 1, 3, 4
  \ and 6, which are organized as an array of 128 sector
  \ buffers, each of 512 bytes. The cache and the RAM disk
  \ occupy two separate (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See: `set-1346`, `default-1346`, `bank`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

unneeding set-1346 ?(

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
  \ set-1346 ( n1 n2 n3 n4 -- ) "set-1-3-4-6"
  \
  \ Set the +3DOS configuration of RAM banks 1, 3, 4 and 6,
  \ which are organized as an array of 128 sector buffers, each
  \ of 512 bytes. The cache and the RAM disk occupy two
  \ separate (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See: `get-1346`, `default-1346`, `bank`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

unneeding 2-block-drives ?( need set-block-drives

: 2-block-drives ( -- ) 'B' 'A' 2 set-block-drives ;

2-block-drives ?)

  \ doc{
  \
  \ 2-block-drives ( -- )
  \
  \ Set all drives as block drives, in normal order: `A` and
  \ `B`.
  \
  \ Note: For convenience, when this word is loaded, it's also
  \ executed.
  \
  \ See: `set-block-drives`.
  \
  \ }doc

( (delete-file delete-file )

unneeding (delete-file ?(

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
  \ (delete-file ( ca -- ior ) "paren-delete-file"
  \
  \ Delete the disk file named in the $FF-terminated string
  \ _ca_ and return an error result _ior_.
  \
  \ ``(delete-file`` is a factor of `delete-file`.
  \
  \ }doc

unneeding delete-file ?( need >filename need (delete-file

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
  \ See: `(delete-file`.
  \
  \ }doc

( #file-ids file-ids file-id )

16 cconstant #file-ids

  \ doc{
  \
  \ #file-ids ( -- n )
  \
  \ _n_ is the total number of file identifiers that can be
  \ used.
  \
  \ See: `file-ids`, `file-id`.
  \
  \ }doc

create file-ids #file-ids allot
file-ids #file-ids erase

  \ XXX TODO -- Use also to remember which files are headed,
  \ one bit.

  \ doc{
  \
  \ file-ids ( -- ca )
  \
  \ _ca_ is the address of a byte table containing the status
  \ of the file identifiers:
  \
  \ |===
  \ | $00 | Not used
  \ | $FF | Used
  \ |===
  \
  \ See: `#file-ids`, `file-id`.
  \
  \ }doc

: file-id ( -- fid true | false )
  #file-ids 0 ?do
    i file-ids + dup c@ 0=
    if $FF swap c! i true unloop exit then drop
  loop false ;


  \ doc{
  \
  \ file-id ( -- fid true | false )
  \
  \ If there is a file identifier not used yet, return it _fid_
  \ and _true_; otherwise return _false_.
  \
  \ See: `#file-ids`, file-ids`.
  \
  \ }doc

( r/o w/o r/w s/r bin headed do-dos-open_ )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

unneeding r/o ?\ %001 cconstant r/o

  \ doc{
  \
  \ r/o ( -- fam ) "r-o"
  \
  \ Return the "read only" file access method _fam_.
  \
  \ See: `w/o`, `r/w`, `s/r`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

unneeding w/o ?\ %010 cconstant w/o

  \ doc{
  \
  \ w/o ( -- fam ) "w-o"
  \
  \ Return the "write only" file access method _fam_.
  \
  \ See: `r/o`, `r/w`, `s/r`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

unneeding r/w ?\ %011 cconstant r/w

  \ doc{
  \
  \ r/w ( -- fam ) "r-w"
  \
  \ Return the "read/write" file access method _fam_.
  \
  \ See: `r/o`, `w/o`, `s/r`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

unneeding s/r ?\ %101 cconstant s/r

  \ doc{
  \
  \ s/r ( -- fam ) "s-r"
  \
  \ Return the "shared read" file access method _fam_.
  \
  \ See: `r/o`, `w/o`, `r/w`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ }doc

unneeding bin ?\ need alias ' noop alias bin immediate

  \ doc{
  \
  \ bin ( fam1 -- fam2 )
  \
  \ Modify file access method _fam1_ to additionally select  a
  \ "binary", i.e., not line oriented, file access method,
  \ giving file access method _fam2_.
  \
  \ See: `r/o`, `w/o`, `r/w`, `s/r`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

code headed ( fam1 -- fam2 )
  E1 c, CB c, C0 08 07 * + 05 + c, pushhl jp, end-code ?)
  \ pop hl
  \ set 7,l
  \ jp pushl
  \
  \ Equivalent code in Forth:
  \
  \ : headed ( fam1 -- fam2 ) 128 and ;

  \ doc{
  \
  \ headed ( fam1 -- fam2 )
  \
  \ Modify file access method _fam1_ to additionally select a
  \ "headed", i.e., with an additional +3DOS header, file
  \ access method, giving file access method _fam2_.
  \
  \ See: `bin`, `r/o`, `w/o`, `r/w`, `s/r`,
  \ `create-file`, `open-file`.
  \
  \ }doc

unneeding do-dos-open_ ?( need assembler

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
  b pop, pushdosior nc? ?jp,
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
  \ doc{
  \
  \ create-file ( ca len fam -- fid ior )
  \
  \  Create the file named in the character string specified by
  \  _ca len_, and open it with file access method _fam_.  If a
  \  file with the same name already exists, recreate it as an
  \  empty file.
  \
  \ If the  file  was  successfully created  and  opened, _ior_
  \ is  zero,  _fid_,  is  its identifier, and the file has
  \ been positioned to the start of the file.
  \
  \ Otherwise, _ior_ is a +3DOS I/O result code and _fid_ is
  \ undefined.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See: `open-file`,  `r/o`, `w/o`, `r/w`, `s/r`, `bin`.
  \
  \ }doc

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

  \ doc{
  \
  \ open-file ( ca len fam -- fid ior )
  \
  \  Open the file named in the character string specified by
  \  _ca len_, and open it with file access method _fam_.
  \
  \ If the  file  was  successfully and  opened, _ior_ is zero,
  \ _fid_,  is  its identifier, and the file has been
  \ positioned to the start of the file.
  \
  \ Otherwise, _ior_ is a +3DOS I/O result code and _fid_ is
  \ undefined.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See: `close-file`, `create-file`, `r/o`, `w/o`, `r/w`,
  \ `s/r`, `bin`.
  \
  \ }doc

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
  \ (close-file ( fid -- ior ) "paren-close-file"
  \
  \ Close the file identified by _fid_ and return error result
  \ _ior_.
  \
  \ ``(close-file`` is a factor of `close-file`.
  \ ``(close-file`` closes the file, but does not update
  \ `file-ids`.
  \
  \ }doc

: close-file ( fid -- ior )
  dup >r (close-file dup 0<> file-ids r> + c! ;

  \ doc{
  \
  \ close-file ( fid -- ior )
  \
  \ Close the file identified by _fid_ and return error result
  \ _ior_.
  \
  \ See: `open-file`, `create-file`, `(close-file`.
  \
  \ }doc

( file-position reposition-file )

unneeding file-position ?(

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
  \ See: `reposition-file`, `open-file`, `create-file`.
  \
  \ }doc

unneeding reposition-file ?(

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
  \ See: `file-position`, `open-file`, `create-file`.
  \
  \ }doc

( (cat )

need >filename need /base-filename need /filename-ext

13 cconstant /cat-entry

  \ doc{
  \
  \ /cat-entry ( -- n ) "slash-cat-entry"
  \
  \ Return size _n_, in bytes, of every entry of the
  \ `cat-buffer` used by `cat`.
  \
  \ See: `cat-entries`.
  \
  \ }doc

create cat-entries 16 c,

  \ doc{
  \
  \ cat-entries ( -- ca )
  \
  \ A character variable. _ca_ is the address of a character
  \ containing the number of entries (minimum 2) of the
  \ `cat-buffer`, and used by `(cat`.  Its default value is 16.
  \
  \ See: `/cat-entry`.
  \
  \ }doc

defer cat-buffer ' pad ' cat-buffer defer!

  \ doc{
  \
  \ cat-buffer ( -- a )
  \
  \ A deferred word that returns the address _a_ of the
  \ catalogue buffer, used by `(cat`, `.cat`, `.acat` and other
  \ words.
  \
  \ WARNING: By default, ``cat-buffer`` executes `pad`.
  \
  \ See: `cat-entries`, `/cat-entry`.
  \
  \ }doc

: .filename-ext ( ca -- ) '.' emit /filename-ext type ;

  \ doc{
  \
  \ .filename-ext ( -- ca ) "dot-filename-ext"
  \
  \ Display the filename extension whose `/filename-ext`
  \ characters (left justified, space filled) are stored at
  \ _ca_. A dot separator is printed first, which is not
  \ included in the string at _ca_.
  \
  \ See: `.filename`.
  \
  \ }doc

: .filename ( ca -- )
  /base-filename 2dup type + .filename-ext ; -->

  \ doc{
  \
  \ .filename ( ca -- ) "dot-filename"
  \
  \ Display the filename whose characters are stored at _ca_,
  \ in two parts: `/base-filename` characters (left justified,
  \ space filled) followed by `/filename-ext` characters (left
  \ justified, space filled).  The dot that separates the base
  \ filename from the filename extension is not included in the
  \ string at _ca_, but it's printed.
  \
  \ See: `.filename-ext`.
  \
  \ }doc

( (cat )

: >cat-entry ( n -- ca ) /cat-entry * cat-buffer + ;

  \ doc{
  \
  \ >cat-entry ( n -- ca ) "to-cat-entry"
  \
  \ Convert `cat-buffer` entry _n_ to its address _ca_.
  \
  \ See: `/cat-entry`.
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
  \ See: `>cat`.
  \
  \ }doc

: >cat ( ca len -- ca1 ca2 x )
  >filename cat-buffer dup /cat-entry erase
            cat-entries c@ 1+ $100 * full-cat @ abs or ;

  \ doc{
  \
  \ >cat ( ca len -- ca1 ca2 x ) "to-cat"
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

  \ See: `wcat`, `cat`, `cat-entries`.
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
  \ See: `(cat`.
  \
  \ }doc

: more-cat? ( n -- f ) cat-entries c@ 1+ = ;

  \ doc{
  \
  \ more-cat? ( n -- f ) "more-cat-question"
  \
  \ There may be more catalague entries to come after _n_ of
  \ them have been completed in `cat-buffer`?
  \
  \ ``more-cat?`` is a factor of `wcat` and `wacat`.
  \
  \ See: `(cat`.
  \
  \ }doc

code (cat ( ca1 ca2 x -- n ior )
  D9 c, C1 c, D1 c, E1 c,
    \ exx, b pop, d pop, h pop,
  DD c, 21 c, 011E , dos-ix_ call, 48 c, 06 c, 00 c, C5 c,
    \ 011E ix ldp#, dos-ix_ call, b c ld, 0 b ld#, b push,
  D9 c, pushdosior jp, end-code
    \ exx, pushdosior jp, end-code

  \ doc{
  \
  \ (cat ( ca1 ca2 x -- n ior ) "paren-cat"
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
  \ See: `cat-buffer`, `cat-entries`, `/cat-entry`.
  \
  \ }doc

( wcat cat )

need (cat need 3dup need 3drop

: .cat-entry ( ca -- )
  dup .filename
      /base-filename /filename-ext + + @ 4 .r ."  KiB" ;

  \ doc{
  \
  \ .cat-entry ( ca -- ) "dot-cat-entry"
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
  \ See: `.cat`, `.filename`.
  \
  \ }doc

: .cat ( n -- ) 1 ?do  i >cat-entry .cat-entry cr loop ;

  \ doc{
  \
  \ .cat ( n -- ) "dot-cat"
  \
  \ Display _n_ entries from `cat-buffer`.
  \
  \ ``.cat`` is a factor of `wcat`.
  \
  \ See: `.cat-entry`, `.acat`.
  \
  \ }doc

: wcat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                          while  dup .cat more-cat?
                          while  more-cat
                          repeat then 3drop ;

  \ doc{
  \
  \ wcat ( ca len -- ) "w-cat"
  \
  \ Show a wild-card disk catalogue using the wild-card
  \ filename _ca len_.
  \
  \ See: `cat`, `wacat`, `(cat`, `more-cat`, `set-drive`.
  \
  \ }doc

: cat ( -- ) s" *.*" wcat ;

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See: `wcat`, `acat`, `(cat`, `set-drive`.
  \
  \ }doc

( wacat acat )

need (cat need tab need 3dup need 3drop

: .acat ( n -- ) 1 ?do  i >cat-entry .filename tab loop ;

  \ doc{
  \
  \ .acat ( n -- ) "dot-a-cat"
  \
  \ Display _n_ entries from `cat-buffer`, in abbreviated
  \ format.
  \
  \ ``.acat`` is a factor of `wacat`.
  \
  \ See: `.filename`, `.cat`.
  \
  \ }doc

: wacat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                           while  dup .acat more-cat?
                           while  more-cat
                           repeat then 3drop ;

  \ doc{
  \
  \ wacat ( ca len -- ) "w-a-cat"
  \
  \ Show an abbreviated wild-card disk catalogue using the
  \ wild-card filename _ca len_.
  \
  \ See: `acat`, `wcat`, `(cat`, `more-cat`, `set-drive`.
  \
  \ }doc

: acat ( -- ) s" *.*" wacat ;

  \ doc{
  \
  \ acat ( -- ) "a-cat"
  \
  \ Show an abbreviated disk catalogue of the current drive.
  \
  \ See: `wacat`, `cat`, `(cat`, `set-drive`.
  \
  \ }doc

( bank-write-file write-file )

unneeding bank-write-file ?(

code bank-write-file ( ca len fid +n -- ior )

  E1 c, 78 05 + c, save-ip_ call,
    \ pop hl ; l = bank
    \ ld a,l
    \ bank_write_file_.a:
    \ call save_ip
  48 07 + c, E1 c, 40 05 + c, D1 c, E1 c, DD c, 21 c, 0115 ,
    \ ld c,a
    \ pop hl
    \ ld b,l ; fid
    \ pop de ; len
    \ pop hl ; ca
    \ ld ix,dos_write ; $0115
  dos-ix_ call, restore-ip_ call, pushdosior jp, end-code ?)
    \ call dos.ix
    \ call restore_ip
    \ jp push_dos_ior

  \ Credit:
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ bank-write-file ( ca len fid +n -- ior )
  \
  \ Write _len_ characters from address _ca_ to the file
  \ identified by _fid_ starting at its current position, while
  \ memory bank _+n_ is paged in addresses $C000..$FFFF.
  \ Return input/output result _ior_.
  \
  \ See: `write-file`, `bank`, `create-file`, `open-file`.
  \
  \ }doc

unneeding write-file ?( need bank-write-file

code write-file ( ca len fid -- ior )
  3A c, 5B5C , E6 c, %111 c, ' bank-write-file 2+ jp,
  end-code ?)
  \ ld a,(sys_bankm) ; $5B5C
  \ and %111 ; current page for 0xC000..0xFFFF
  \ jp bank_write_file_.a

  \ Credit:
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ write-file ( ca len fid -- ior )
  \
  \ Write _len_ characters from address _ca_ to the file
  \ identified by _fid_ starting at its current position.
  \ Return input/output result _ior_.
  \
  \ See: `bank-write-file`, `create-file`, `open-file`.
  \
  \ }doc

( bank-read-file read-file )

  \ XXX UNDER DEVELOPMENT

unneeding bank-read-file ?(

code bank-read-file  ( ca len fid +n -- ior )
  \ XXX TODO --

  \ XXX FIXME
  \
  \ +3DOS causes EOF error when the desired length is beyond
  \ the end of file (not counting the padding 0x1A at the end).
  \ This makes it impossible to behave according to Forth-94.
  \
  \ Solution: check the file position at the start.

  \   pop hl ; l = bank
  \   ld a,l
  \ page_read_file_.a:
  \   call save_ip
  \   ld c,a
  \   pop hl
  \   ld b,l  ; fid
  \   pop de  ; len
  \   pop hl  ; ca
  \   push de ; len
  \   ld ix,dos_read
  \   call dos
  \   pop hl ; len
  \   call restore_ip
  \   jr c,page_read_file_.no_error

  \   ; error
  \   ; hl = len
  \   ; a = error code
  \   ; de = number of bytes remaining unread

  \   push af
  \   call hl_minus_de_to_hl
  \   ; hl = bytes actually read
  \   pop af
  \   cp 21 ; is it "bad parameter"? XXX TODO label
  \   jr z, page_read_file_.no_error
  \   push hl
  \   jp back_from_dos.error

  \ page_read_file_.no_error:
  \   ; no error
  \   ; hl = len
  \   push hl
  \   jp false_

  end-code ?)

  \
  \ bank-read-file ( ca len fid +n -- ior )
  \
  \ Read _len_ consecutive  characters to  _ca_ from the
  \ current position  of the  file identified by _fid_ with
  \ bank _+n_ paged in address range $C000..$FFFF.
  \
  \ If _len1_ characters are read without an exception, _ior_
  \ is zero and _len2_ is equal to _len1_.
  \
  \ If the end of the file is reached before _len1_ characters
  \ are read, _ior_ is zero and _len2_ is the number of
  \ characters actually read.
  \
  \ If the operation is initiated when the value returned by
  \ `file-position` is equal to the value returned by
  \ `file-size` for the file identified by fileid, _ior_ is
  \ zero and _len2_ is zero.
  \
  \ If an exception occurs, _ior_ is the implementation-defined
  \ I/O result code, and _len2_ is the number of characters
  \ transferred to _ca_ without an exception.
  \
  \ An ambiguous condition exists if the operation is initiated
  \ when the value returned by `file-position` is greater than
  \ the value returned by `file-size` for the file identified
  \ by fileid, or if the requested operation attempts to read
  \ portions of the file not written.
  \
  \ At the conclusion of the operation, `file-position` returns
  \ the next file position after the last character read.
  \
  \ See: `bank-read-file`, `open-file`, `write-file`.
  \
  \ XXX TODO -- Finish adapting the documentation.

code read-file  ( ca len1 fid -- len2 ior )
  3A c, 5B5C , E6 c, %111 c, ' bank-read-file 2+ jp,
  end-code ?)
  \ ld a,(sys_bankm) ; $5B5C
  \ and %111 ; current page for 0xC000..0xFFFF
  \ jp bank_write_file_.a

  \ Credit:
  \ Adapted from DZX-Forth.

  \
  \ read-file ( ca len1 fid -- len2 ior )
  \
  \ Read _len1_ consecutive  characters to  _ca_ from the
  \ current position  of the  file identified by _fid_.
  \
  \ If _len1_ characters are read without an exception, _ior_
  \ is zero and _len2_ is equal to _len1_.
  \
  \ If the end of the file is reached before _len1_ characters
  \ are read, _ior_ is zero and _len2_ is the number of
  \ characters actually read.
  \
  \ If the operation is initiated when the value returned by
  \ `file-position` is equal to the value returned by
  \ `file-size` for the file identified by fileid, _ior_ is
  \ zero and _len2_ is zero.
  \
  \ If an exception occurs, _ior_ is the implementation-defined
  \ I/O result code, and _len2_ is the number of characters
  \ transferred to _ca_ without an exception.
  \
  \ An ambiguous condition exists if the operation is initiated
  \ when the value returned by `file-position` is greater than
  \ the value returned by `file-size` for the file identified
  \ by fileid, or if the requested operation attempts to read
  \ portions of the file not written.
  \
  \ At the conclusion of the operation, `file-position` returns
  \ the next file position after the last character read.
  \
  \ See: `bank-read-file`, `open-file`, `write-file`.
  \
  \ XXX TODO -- Finish adapting the documentation.

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
  \ to a configurable disposable location, by making
  \ `cat-buffer` a deferred word that executes `pad`.  The
  \ reason of the catalogue corruption was the buffer was
  \ overwritten by other strings during the process. Remove
  \ `allocate-cat-buffer`. Fix documentation. Rewrite `(cat`
  \ with Z80 opcodes.
  \
  \ 2017-12-05: Improve documentation. Rewrite `headed` in Z80.
  \ Fix Z80 opcode in `do-dos-open_`.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-01-11: Improve documentation.
  \
  \ 2018-02-04: Fix documentation. Improve documentation: add
  \ pronunciation to words that need it.
  \
  \ 2018-02-14: Add `bank-write-file` and `write-file`. Prepare
  \ `bank-read-file` and `read-file`.
  \
  \ 2018-03-04: Update documentation of `set-1346` and
  \ `get-1346`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-11: Add `2-block-drives`.
  \
  \ 2018-03-13: Add `#file-ids`. Rename `file-id-table`
  \ `file-ids`. Improve documentation.

  \ vim: filetype=soloforth
