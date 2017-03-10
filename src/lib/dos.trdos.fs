  \ dos.trdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703110045

  \ -----------------------------------------------------------
  \ Description

  \ TR-DOS support.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-08-04: Start: `dos-drive`, `get-drive`, `set-drive`.
  \
  \ 2017-02-05: Start from scratch. Move `get-drive` from the
  \ TR-DOS kernel and rewrite it in Z80. Move `cat` from the
  \ TR-DOS kernel.
  \
  \ 2017-02-10: Add DOS command constants. Add draft words to
  \ manipulate files. Rename "filedescriptor" notation to
  \ "fda". Add draft of `file-status`.
  \
  \ 2017-02-11: Fix `cat`.
  \
  \ 2017-02-12: Fix and document `file-status`. Add
  \ `file-exists?`, `file-start`, `file-length`, `file-type`,
  \ `file-dir`.  `file-sectors`, `file-sector`, `file-track`.
  \
  \ 2017-02-13: Fix `file-status`. Fix `file-dir` and rename it
  \ to `file-dir#`, after the changes in the G+DOS
  \ implementation.
  \
  \ 2017-02-14: Add `find-file`.
  \
  \ 2017-02-17: Restore the original TR-DOS notation
  \ "descriptor".  Update cross references.
  \
  \ 2017-03-04: Update the names of the DOS calls, after the
  \ changes in the kernel.
  \
  \ 2017-03-06: Add `>file`, which was removed from the kernel
  \ by mistake on 2017-02-12.
  \
  \ 2017-03-08: Rename `cat` to `acat`, after the names used in
  \ G+DOS. Add `(acat`. Improve calculation of dosior #1 ("no
  \ files"). Add `delete-file`.
  \
  \ 2017-03-10: Adapt the DOS calls to the changes in the
  \ kernel: The C register is not copied to the A register
  \ anymore after returning from the DOS call; instead, the A
  \ register is loaded from the TR-DOS latest error variable.
  \ Add draft of `undelete-file`.  Add `read-file-descriptor`,
  \ `write-file-descriptor`, `read-system-track`, `files/disk`.
  \ Improve documentation.
  \
  \ 2017-03-11: Finish `undelete-file`. Add `.filename`,
  \ `.fda-filename`, `fda-basic?`, `fda-deleted?`,
  \ `fda-empty?`, `cat`, `cat-fda`, `?cat-fda`. Improve
  \ documentation.

( --dos-commands-- )

  \ TR-DOS command codes

$00 cconstant dos-init-interface
$01 cconstant dos-init-drive
$02 cconstant dos-seek-track
$03 cconstant dos-set-sector
$04 cconstant dos-define-buffer
$05 cconstant dos-read-sectors
$06 cconstant dos-write-sectors
$07 cconstant dos-cat
$08 cconstant dos-read-file-descriptor
$09 cconstant dos-write-file-descriptor
$0A cconstant dos-find-file
$0B cconstant dos-create-file
$0C cconstant dos-save-basic-program
$0E cconstant dos-read-file
$12 cconstant dos-delete-file  -->

( --dos-commands-- )

  \ TR-DOS command codes (continued)

$13 cconstant dos-copy-from-hl-to-descriptor
$14 cconstant dos-copy-from-descriptor-to-hl
$15 cconstant dos-test-track
$16 cconstant dos-select-bottom-side
$17 cconstant dos-select-top-side
$18 cconstant dos-read-system-track

: --dos-commands-- ;

( fda /fda )

[unneeded] fda ?(

$5CDD     constant fda
fda       constant fda-filename
fda $08 + constant fda-filetype
fda $09 + constant fda-filestart
fda $0B + constant fda-filelength
fda $0D + constant fda-filesectors
fda $0E + constant fda-filesector
fda $0F + constant fda-filetrack ?)

  \ doc{
  \
  \ fda ( -- ca )
  \
  \ Return the address _ca_ of TR-DOS File Descriptor Area,
  \ which has the following structure:

  \ |===
  \ | Offset | Bytes | Contents
  \
  \ | +0x0   | 8     | File name
  \ | +0x8   | 1     | File type ('B, 'C', 'D', '#'...)
  \ | +0x9   | 2     | Address (or length of a BASIC program)
  \ | +0xB   | 2     | Length in bytes
  \ | +0xD   | 1     | Length in sectors
  \ | +0xE   | 1     | First sector of the file on its first track
  \ | +0xF   | 1     | First track of the file
  \ |===
  \
  \ See also: `/fda`.
  \
  \ }doc

[unneeded] /fda ?\ $10 cconstant /fda

  \ doc{
  \
  \ /fda ( -- b )
  \
  \ Return the length of TR-DOS `fda` (File Descriptor Area).
  \
  \ }doc

( files/disk /filename -filename -fda-filename set-filename )

[unneeded] files/disk ?\ 128 cconstant files/disk

  \ doc{
  \
  \ files/disk  ( -- n )
  \
  \ Return the maximum number _n_ of files on a disk, including
  \ the deleted files, which is 128.
  \
  \ }doc

[unneeded] /filename ?\ 9 cconstant /filename

  \ doc{
  \
  \ /filename ( -- len )
  \
  \ Return the maximum length _len_ of a TR-DOS filename, which
  \ is 9.  In TR-DOS, the last character of the filename
  \ (character offset 8) is the filetype:

  \ |===
  \ | Character | Filetype
  \
  \ | B         | BASIC program
  \ | C         | Code file
  \ | D         | BASIC data array file
  \ | #         | Serial/random access data file
  \ | other     | Defined by the programmer
  \ |===

  \ If the filetype is not specified in a filename, 'C' is
  \ used.
  \
  \ See also: `set-filename`, `fda`.
  \
  \ }doc

[unneeded] -filename ?( need /filename

: -filename ( ca -- )
  /filename 2dup blank + 1- 'C' swap c! ; ?)

  \ doc{
  \
  \ -filename ( ca -- )
  \
  \ Erase the filename stored at _ca_ and set its type to 'C'.
  \
  \ See also: `-fda-filename`, `set-filename`.
  \
  \ }doc

[unneeded] -fda-filename ?( need fda-filename need -filename

: -fda-filename ( -- ) fda-filename -filename ; ?)

  \ doc{
  \
  \ -fda-filename ( -- )
  \
  \ Erase the filename stored at TR-DOS `fda` (File Descriptor
  \ Area) with spaces, and set its type to 'C'.
  \
  \ See also: `-filename`, `set-filename`.
  \
  \ }doc

[unneeded] set-filename ?(

need -fda-filename need /filename need fda

: set-filename ( ca len -- )
  -fda-filename /filename min fda-filename smove ; ?)

  \ doc{
  \
  \ set-filename ( ca len -- )
  \
  \ Store filename _ca len_ into the TR-DOS `fda` (File
  \ Descriptor Area).  If _len_ is greater than 9 characters
  \ (the value returned by `/filename`), 9 is used instead.  If
  \ _ca len_ does not include the file type at the end (at
  \ character offset +8), 'C' (code file) is used by default.
  \
  \ See also: `-fda-filename`, `/filename`.
  \
  \ }doc

( get-drive (acat acat )

[unneeded] get-drive

?\ code get-drive ( -- b ) 3A c, 5CF6 , pusha jp, end-code

  \ Note: $5CF6 is the TR-DOS variable "current temporary
  \ drive".

  \ doc{
  \
  \ get-drive ( -- b )
  \
  \ Return the number _b_ of the current drive (0..3).
  \
  \ This word is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : get-drive ( -- b )
  \   $5FC6 c@ ;
  \ ----

  \ See also: `set-drive`.
  \
  \ }doc

[unneeded] (acat ?(

code (acat ( -- ior )
  3E c, 07 c, 08 c, 3E c, 02 c, dos-alt-a-preserve-ip_ call,
  \ ld a,trdos_command.cat
  \ ex af,af'
  \ ld a,2 ; stream: screen
  \ call dos.alt_a.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ doc{
  \
  \ (acat ( -- ior )
  \
  \ Print an abbreviated catalog of the current disk and return
  \ error result _ior_.  ``(acat`` is a factor of `acat`.
  \
  \ See also: `set-drive`.
  \
  \ }doc

[unneeded] acat ?\ need (acat : acat ( -- ) (acat throw ;

  \ doc{
  \
  \ acat ( -- ior )
  \
  \ Print an abbreviated catalog of the current disk.
  \
  \ See also: `set-drive`, `(acat`.
  \
  \ }doc

( file> )

need assembler need --dos-commands-- need fda need set-filename

code (file>) ( ca len -- ior )

  d pop, h pop, b push, h push, d push, ( ip ca len )

  dos-find-file c ld#, dos-c_ call,
  \ C = directory entry of the file, or $FF if not found
  c a ld, c inc, z? rif d pop, h pop, 1 a ld#, relse
                \ error, so drop parameters
                \ and set dosior #1 ("no files")
    dos-read-file-descriptor c ld#, dos-c_ call,
    a xor, 5CF9 sta, d pop, h pop,
      \ set load flag (0) and get the parameters
    d tstp, nz? rif 03 a ld#,
                    \ DE (_len_) is not zero,
                    \ so use the parameters: make A=$03
                relse h tstp, nz? rif FF a ld#, rthen rthen
                    \ DE (_len_) is zero, so:
                    \   - if HL (_ca_) is non-zero,
                    \     use paremeter address,
                    \     but length from `fda`: make A=$FF
                    \   - if HL (_ca_) is zero,
                    \     use address and length from `fda`:
                    \     nothing is needed, because
                    \     A is already 0 after `tstp,`
    dos-read-file c ld#, dos-c_ call,
      \ read the file
  rthen b pop, pushdosior jp, end-code

  \ Note: TR-DOS command `dos-read-file` works different ways
  \ depending on the A register:
  \
  \ A=$00 - take address and length from FDA
  \ A=$03 - take address from HL and length from DE
  \ A=$FF - take address from HL but length from FDA
  \
  \ FDA = File Descriptor Area

  \ doc{
  \
  \ (file>) ( ca len -- ior )
  \
  \ Search the disk for the file whose filename is stored in
  \ `fda` and read its metadata into `fda`. Then read the file
  \ contents to memory zone _ca len_ or to the original memory
  \ zone of the file, depending on the following rules:
  \
  \ 1. If _len_ is not zero, read _len_ bytes from the file to
  \ address _ca_.
  \
  \ 2. If _len_ is zero, use the original length of the file
  \ insted, and then check _ca_: If _ca_ is not zero, use it as
  \ destination address, else use the original address of the
  \ file.
  \
  \ Return error result _ior_.
  \
  \ ``(file>)`` is a factor of `file>`.
  \
  \ See also: `fda-filestart`, `fda-filelength`.
  \
  \ }doc

: file> ( ca1 len1 ca2 len2 -- ior )
  2swap set-filename (file>) ;

  \ doc{
  \
  \ file> ( ca1 len1 ca2 len2 -- ior )
  \
  \ Read the contents of a disk file, whose filename is defined
  \ by the string _ca1 len1_, to memory zone _ca2 len2_ (i.e.
  \ read _len2_ bytes and store them starting at address
  \ _ca2_), or use the original address and length of the file
  \ instead, depending on the following rules:
  \
  \ 1. If _len2_ is not zero, use _ca2 len2_.
  \
  \ 2. If _len2_ is zero, use the original file length instead
  \ and then check also _ca2_: If _ca2_ is zero, use the
  \ original file address instead.
  \
  \ Return error result _ior_.
  \
  \ Example:
  \
  \ The screen memory (start address 16384 and size 6912 bytes)
  \ is saved to a disk file with `>file`:

  \ ----
  \ 16384 6912 s" pic.scr" >file
  \ ----

  \ Now there are several ways to load that file from disk:

  \ |===
  \ | Example                        | Result
  \
  \ | `s" pic.scr" 16384 6912 file>` | Load the file using its original known values
  \ | `s" pic.scr" 16384 6144 file>` | Load only the bitmap to the original known address
  \ | `s" pic.scr"     0    0 file>` | Load the file using its original unknown values
  \ | `s" pic.scr" 32768    0 file>` | Load the whole file to address 32768
  \ | `s" pic.scr" 32768  256 file>` | Load only 256 bytes to address 32768
  \ |===

  \
  \ }doc

( >file )

need assembler need l: need --dos-commands--
need fda need set-filename

code (>file) ( -- ior )

  b push, dos-read-system-track c ld#, dos-c_ call,
  0 l# nz? ?jr,
  \   push bc ; save the Forth IP
  \
  \   ld c,trdos_command.read_system_track
  \   call dos.c
  \   jr nz,paren_to_file.exit ; jump if error
  \
  \   ; XXX FIXME -- 2017-02-10: When there's no disk in the drive,
  \   ; TR-DOS throws "Disc Error. Retry,Abort,Ignore?". "Retry" is
  \   ; useless; "Abort" exits to BASIC with "Tape loading error";
  \   ; "Ignore" crashes the system. The call to `read_system_track`
  \   ; does not return.
  \   ;
  \   ; There must be a way to avoid this and return an ior.
  \
  dos-find-file c ld#, dos-c_ call, c inc, nz? rif
  \   ld c,trdos_command.find_file
  \   call dos.c
  \   ; C = directory entry of the file (0..127), or $FF if not found
  \   inc c ; file not found?
  \   jr z,paren_to_file.file_not_found ; jump if no error
  \
  \   ; error: file found
  2 a ld#, 0 l# jr,
  \   ld a,trdos_error.file_exists ; error code "file exists"
  \   jr paren_to_file.exit

  rthen fda-filelength d ftp, fda-filestart h ftp,
  dos-create-file c ld#, dos-c_ call,
  \ paren_to_file.file_not_found:
  \   ld de,(fda.filelength)
  \   ld hl,(fda.filestart)
  \   ld c,trdos_command.create_file
  \   call dos.c

  0 l: b pop, pushdosior jp, end-code
  \ paren_to_file.exit:
  \   pop bc  ; restore the Forth IP
  \   jp push_dos_ior

: >file ( ca1 len1 ca2 len2 -- ior )
  set-filename fda-filelength ! fda-filestart ! (>file) ;

( file-status )

need assembler need --dos-commands--
need fda need set-filename

code (file-status) ( -- a ior )

  fda h ldp#, h push, b push,
    \ Push `fda` (the _a_ returned) and save Forth IP.
  dos-find-file a ld#, exaf, dos-alt-a_ call,
    \ C = directory entry (0..127), or $FF if file not found
  c a ld, c inc, z? rif  1 a ld#,  \ dosior #1 ("no files")
  relse  dos-read-file-descriptor c ld#, dos-c_ call,
  rthen b pop, pushdosior jp, end-code

: file-status ( ca len -- a ior )
  set-filename (file-status) ;

  \ doc{
  \
  \ file-status ( ca len -- a ior )
  \
  \ Return the status of the file identified by the character
  \ string _ca len_. If the file exists, _ior_ is zero and _a_
  \ is the address returned by `fda`, the TR-DOS File
  \ Descriptor Area; otherwise _ior_ is the corresponding I/O
  \ result code and _a_ is useless.
  \
  \ Origin: Forth-94 (FILE-EXT), Forth-2012 (FILE-EXT).
  \
  \ }doc

( file-exists? file-start file-length file-type find-file )

[unneeded] file-exists? ?( need file-status

: file-exists? ( ca len -- f ) file-status nip 0= ; ?)

  \ doc{
  \
  \ file-exists? ( ca len -- f )
  \
  \ If the file named in the character string _ca len_ is
  \ found, _f_ is _true_. Otherwise _f_ is _false_.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-start  ?( need file-status need fda

: file-start ( ca1 len1 -- ca2 ior )
  file-status nip fda-filestart @ swap ; ?)

  \ doc{
  \
  \ file-start ( ca1 len1 -- ca2 ior )
  \
  \ Return the file start address of the file named in the
  \ character string _ca1 len1_. If the file was successfully
  \ found, _ior_ is zero and _ca2_ is the start address.
  \ Otherwise _ior_ is an exception code and _ca2_ is
  \ undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-len  ?( need file-status need fda

: file-length ( ca1 len1 -- len2 ior )
  file-status nip fda-filelength @ swap ; ?)

  \ doc{
  \
  \ file-length ( ca1 len1 -- len2 ior )
  \
  \ Return the file length of the file named in the character
  \ string _ca1 len1_. If the file was successfully found,
  \ _ior_ is zero and _len2_ is the file length.  Otherwise
  \ _ior_ is an exception code and _len2_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-type  ?( need file-status need fda

: file-type ( ca len -- n ior )
  file-status nip fda-filetype c@ swap ; ?)

  \ XXX REMARK -- 2017-02-13: This word is uselsin TR-DOS,
  \ because the filetype is part of the filename.

  \ doc{
  \
  \ file-type ( ca len -- c ior )
  \
  \ Return the TR-DOS file-type indentifier of the file named
  \ in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the file-type
  \ identifier.  Otherwise _ior_ is an exception code and _n_
  \ is undefined.
  \
  \ Note this word is useless in TR-DOS, because the file type
  \ in part of the filename. This word is included for
  \ compatibility with other DOSs supported.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] find-file ?( need file-status
: find-file ( ca len -- f ) file-status 0= and ; ?)

  \ doc{
  \
  \ find-file ( ca len -- a | 0 )
  \
  \ If the file named in the character string _ca len_ is
  \ found, return address _a_ of the updated `fda` (File
  \ Descriptor Area). Otherwise return zero.
  \
  \ See also: `file-status`.
  \
  \ }doc

( file-dir# )

need assembler need --dos-commands-- need set-filename

code (file-dir#) ( -- n ior )

  dos-find-file a ld#, exaf, dos-preserve-ip_ call,
    \ A = directory entry (0..127), or $FF if file not found
  0 h ld#, a l ld, h push,
  a inc, z? rif   a inc, \ dosior #1 ("no files")
            relse a xor, rthen
  pushdosior jp, end-code

  \ doc{
  \
  \ (file-dir#) ( -- n ior )
  \
  \ Return the file directory number of the file whose filename
  \ is stored at `fda` (File Descriptor Area). If the file was
  \ successfully found, _ior_ is zero and _n_ is the file
  \ directory number.  Otherwise _ior_ is an exception code and
  \ _n_ is undefined.
  \
  \ See also: `file-dir#`, `file-status`.
  \
  \ }doc

: file-dir# ( ca len -- n ior ) set-filename (file-dir#) ;

  \ doc{
  \
  \ file-dir# ( ca len -- n ior )
  \
  \ Return the file directory number of the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the file directory number.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

( file-sectors file-sector file-track )

[unneeded] file-sectors  ?( need file-status need fda

: file-sectors ( ca len -- n ior )
  file-status nip fda-filesectors c@ swap ; ?)

  \ doc{
  \
  \ file-sectors ( ca len -- n ior )
  \
  \ Return the sectors occupied by the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the length in sectors.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-sector  ?( need file-status need fda

: file-sector ( ca len -- n ior )
  file-status nip fda-filesector c@ swap ; ?)

  \ doc{
  \
  \ file-sector ( ca len -- n ior )
  \
  \ Return the first sector of the first track of the file
  \ named in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the sector.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-track  ?( need file-status need fda

: file-track ( ca len -- n ior )
  file-status nip fda-filetrack c@ swap ; ?)

  \ doc{
  \
  \ file-track ( ca len -- n ior )
  \
  \ Return the first track of the file named in the character
  \ string _ca len_. If the file was successfully found, _ior_
  \ is zero and _n_ is the track.  Otherwise _ior_ is an
  \ exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

( delete-file )

need assembler need --dos-commands--
need fda need set-filename

code (delete-file) ( -- ior )
  b push,
  dos-find-file c ld#, dos-c_ call,
  \ C = directory entry of the file, or $FF if not found
  c a ld, c inc, z? rif  1 a ld#, \ dosior #1 ("no files")
                    relse dos-delete-file c ld#, dos-c_ call,
                    rthen b pop, pushdosior jp, end-code

  \ doc{
  \
  \ (delete-file) ( -- ior )
  \
  \ Delete a disk file using the data hold in `dfa`.
  \ Return an error result _ior_.
  \
  \ ``(delete-file)`` is a factor of `delete-file`.
  \
  \ }doc

: delete-file ( ca len -- ior ) set-filename (delete-file) ;

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

( read-system-track )

need assembler need --dos-commands--

code read-system-track ( -- ior )
  dos-read-file-descriptor a ld#, exaf,
  dos-alt-a-preserve-ip_ call, pushdosior jp, end-code

( read-file-descriptor write-file-descriptor )

need assembler need --dos-commands--

code read-file-descriptor ( n -- ior )
  h pop, b push, l a ld,
  dos-read-file-descriptor c ld#, dos-c_ call,
  b pop, pushdosior jp, end-code
  \ XXX TODO -- Rename to `entry>fda`?

code write-file-descriptor ( n -- ior )
  h pop, b push, l a ld,
  dos-write-file-descriptor c ld#, dos-c_ call,
  b pop, pushdosior jp, end-code
  \ XXX TODO -- Rename to `fda>entry`?

( undelete-file )

need files/disk

create tmp-filename /filename allot

: undelete-file ( ca len -- ior )
  tmp-filename -filename tmp-filename smove 1 tmp-filename c!
  read-system-track throw  files/disk 0 ?do
    i read-file-descriptor throw fda-filename /filename
                                 tmp-filename /filename str=
    if   $5D08 c@ fda-filename c! i write-file-descriptor
         unloop exit then
  loop #-1001 ;

  \ XXX TODO -- Improve: `read-file-descriptor` reads the
  \ system track every time. Explore the sector buffer instead.

  \ doc{
  \
  \ undelete-file ( ca len -- ior )
  \
  \ Undelete the disk file named in the string _ca len_ and
  \ return an error result _ior_.
  \
  \ TR-DOS deletes a file replacing its first character with
  \ byte 1.  ``undelete-file`` replaces the first character in
  \ _ca len_ with byte 1, then searches the disk for such
  \ filename and restores its first character using the first
  \ character removed from the latest deleted file, which
  \ TR-DOS keeps in its variable $5D08.
  \
  \ Therefore, the procedure has some issues:
  \
  \ 1. If _ca len_ is not the latest deleted file, the first
  \ character of its filename will not be the original one.
  \
  \ 2. If more than one file has been deleted, with only the
  \ first character of their filenames being different in all
  \ of them, ``undelete-file`` will find the oldest one.
  \
  \ 3. TR-DOS does not reuse the space occupied by a deleted
  \ file, until the disk is defragmented
  \
  \ See also: `delete-file`.
  \
  \ }doc

  \ XXX REMARK -- The TR-DOS command `dos-find-file` can not
  \ locate deleted files, it ignores them, i.e.  filenames with
  \ a byte 1 as first char. That's why an alternative was
  \ needed.

( undelete-file )

  \ XXX OLD -- 2017-03-10. First try.
  \
  \ XXX FIXME -- `dos-find-file` can not locate deleted files,
  \ it ignores them, i.e.  filenames with a byte 1 as first
  \ char.  A custom procedure must be written.

need assembler need --dos-commands--
need fda need set-filename

code (undelete-file ( -- ior )
  b push,
  dos-find-file c ld#, dos-c_ call,
  \ C = directory entry of the file, or $FF if not found
  c a ld, c inc, z?
  rif   1 a ld#, \ dosior #1 ("no files")
  relse af push, dos-read-file-descriptor c ld#, dos-c_ call,
                 5D08 fta, fda-filename sta,
        af pop,  dos-write-file-descriptor c ld#, dos-c_ call,
  rthen b pop, pushdosior jp, end-code

  \ doc{
  \
  \ (delete-file) ( -- ior )
  \
  \ Delete a disk file using the data hold in `dfa`.
  \ Return an error result _ior_.
  \
  \ ``(delete-file)`` is a factor of `delete-file`.
  \
  \ }doc

: undelete-file ( ca len -- ior )
  set-filename 1 fda-filename c! (undelete-file ;

  \ doc{
  \
  \ undelete-file ( ca len -- ior )
  \
  \ Undelete the disk file named in the string _ca len_ and
  \ return an error result _ior_.
  \
  \ TR-DOS deletes a file replacing its first character with
  \ byte 1.  ``undelete-file`` replaces the first character in
  \ _ca len_ with byte 1, then searches the disk for such
  \ filename and restores its first character using the first
  \ character removed from the latest deleted file, which
  \ TR-DOS keeps in its variable $5D08.
  \
  \ Therefore, the procedure has some issues:
  \
  \ 1. If _ca len_ is not the latest deleted file, the first
  \ character of its filename will not be the original one.
  \
  \ 2. If more than one file has been deleted, with only the
  \ first character of their filenames being different in all
  \ of them, ``undelete-file`` will find the oldest one.
  \
  \ 3. TR-DOS does not reuse the space occupied by a deleted
  \ file, until the disk is defragmented
  \
  \ See also: `delete-file`.
  \
  \ }doc

( .filename .fda-filename fda-basic? fda-deleted? fda-empty? )

[unneeded] .filename ?( need /filename

: .filename ( ca -- )
  /filename 1- 2dup type '<' emit + c@ emit '>' emit ; ?)
  \ XXX TODO -- Call the ROM routine instead.

  \ doc{
  \
  \ .filename ( ca -- )
  \
  \ Display the filename stored at _ca_, using the TR-DOS
  \ filename format.
  \
  \ See also: `.fda-filename`, `/filename`.
  \
  \ }doc

[unneeded] .fda-filename ?( need fda need .filename

: .fda-filename ( -- ) fda-filename .filename ; ?)

  \ doc{
  \
  \ .fda-filename ( -- )
  \
  \ Display the contents of `fda-filename`, using the TR-DOS
  \ filename format.
  \
  \ See also: `.filename`, `/filename`.
  \
  \ }doc

[unneeded] fda-basic?

?\ need fda : fda-basic? ( -- f ) fda-filetype c@ 'B' = ;

  \ doc{
  \
  \ fda-basic? ( -- f )
  \
  \ _f_ is true if `fda` contains a BASIC program file.
  \
  \ See also: `fda-empty?`, `fda-deleted?`.
  \
  \ }doc

[unneeded] fda-deleted?

?\ need fda : fda-deleted? ( -- ) fda-filename c@ 1 = ;

  \ doc{
  \
  \ fda-deleted? ( -- f )
  \
  \ _f_ is true if `fda` contains a deleted file.
  \
  \ See also: `fda-empty?`, `fda-basic?`.
  \
  \ }doc

[unneeded] fda-empty?

?\ need fda : fda-empty? ( -- f ) fda c@ 0= ;

  \ doc{
  \
  \ fda-empty? ( -- f )
  \
  \ _f_ is true if `fda` is empty, i.e. it's unused, it does
  \ not contain a file descriptor.
  \
  \ See also: `fda-deleted?`, `fda-basic?`.
  \
  \ }doc

( cat )

need --dos-commands-- need files/disk need read-system-track
need read-file-descriptor need u.r need fda need .fda-filename
need fda-basic? need fda-empty? need fda-deleted?

: cat-fda ( n -- )
  3 .r space .fda-filename fda-filesectors c@ 3 u.r
  6 fda-basic? if spaces else fda-filestart @ swap u.r then
  fda-filelength @ 6 u.r cr ;

  \ XXX TODO -- Add the BASIC autorun line. The problem is that
  \ information is not in File Descriptor Area, but at the
  \ start of the file contents, on its first sector. See TR-DOS
  \ routine at $131B.

  \ doc{
  \
  \ cat-fda ( n -- )
  \
  \ Display catalogue entry _n_ of the current drive.
  \ The entry is already stored in `fda`.
  \
  \ ``cat-fda`` is a factor of `?cat-fda`.
  \
  \ See also: `.fda-filename`, `fda-basic?`.
  \
  \ }doc

: ?cat-fda ( n -- ) fda-deleted? if drop exit then cat-fda ;

  \ doc{
  \
  \ ?cat-fda ( n -- )
  \
  \ If catalogue entry _n_ of the current drive is not a
  \ deleted file, display it.  The entry is already stored in
  \ `fda`.
  \
  \ ``?cat-fda`` is a factor of `cat`.
  \
  \ See also: `fda-deleted?`, `cat-fda`.
  \
  \ }doc

: cat ( -- )
  read-system-track throw  cr
  files/disk 0 ?do i read-file-descriptor throw
                   fda-empty? if unloop exit then i ?cat-fda
               loop ;

  \ XXX TODO -- Improve: `read-file-descriptor` reads the
  \ system track every time. Explore the sector buffer instead.

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See also: `acat`, `?cat-fda`, `cat-fda`, `set-drive`.
  \
  \ }doc

  \ vim: filetype=soloforth
