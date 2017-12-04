#!//usr/bin/env python

# Original source from:
# "Build a digital book with EPUB" by Liza Daly,
# published on 2008-11-25 and updated 2011-07-13 in
# <https://www.ibm.com/developerworks/xml/tutorials/x-epubtut/index.html>

from lxml import etree
import sys, os, os.path, logging, shutil, zipfile

logging.basicConfig(level=logging.DEBUG)
log = logging.getLogger('docbook2epub')

# XXX REMARK -- Original:
#DOCBOOK_XSL = os.path.abspath('../docbook-xsl-1.74.0/epub/docbook.xsl')
# XXX REMARK -- 2017-07-22: Modified by Marcos Cruz (programandala.net):
DOCBOOK_XSL = os.path.abspath('/usr/share/xml/docbook/stylesheet/docbook-xsl/epub/docbook.xsl')

MIMETYPE = 'mimetype'
MIMETYPE_CONTENT = 'application/epub+zip'

xslt_ac = etree.XSLTAccessControl(read_file=True,write_file=True, create_dir=True, read_network=True, write_network=False)
transform = etree.XSLT(etree.parse(DOCBOOK_XSL), access_control=xslt_ac)

def convert_docbook(docbook_file):
    '''Use DocBook XSL to transform our DocBook book into EPUB'''
    cwd = os.getcwd()
    # Create a temporary working directory for the output files
    output_path = os.path.basename(os.path.splitext(docbook_file)[0])
    if not os.path.exists(output_path):
        os.mkdir(output_path)

    # DocBook needs the source file in the current working directory to output correctly
    shutil.copy(docbook_file, output_path)
    os.chdir(output_path)

    # Call the transform
    transform(etree.parse(docbook_file))

    os.chdir(cwd)

    # Return the working directory for the EPUB
    return output_path

def find_resources(path):
    '''Parse the content manifest to find all the resources in this book.'''
    opf = etree.parse(os.path.join(path, 'OEBPS', 'content.opf'))
    # All the <opf:item> elements are resources
    for item in opf.xpath('//opf:item', 
                          namespaces= { 'opf': 'http://www.idpf.org/2007/opf' }):

        # If the resource was not already created by DocBook XSL itself, 
        # copy it into the OEBPS folder
        href = item.attrib['href']
        referenced_file = os.path.join(path, 'OEBPS', href)
        if not os.path.exists(referenced_file):
            log.debug("Copying '%s' into content folder" % href)
            shutil.copy(href, '%s/OEBPS' % path)
    
def create_mimetype(path):
    '''Create the mimetype file'''
    f = '%s/%s' % (path, MIMETYPE)
    f = open(f, 'w')
    f.write(MIMETYPE_CONTENT)
    f.close()

def create_archive(path):
    '''Create the ZIP archive.  The mimetype must be the first file in the archive 
    and it must not be compressed.'''
    cwd = os.getcwd()

    epub_name = '%s.epub' % os.path.basename(path)

    # The EPUB must contain the META-INF and mimetype files at the root, so 
    # we'll create the archive in the working directory first and move it later
    os.chdir(path)    

    # Open a new zipfile for writing
    epub = zipfile.ZipFile(epub_name, 'w')

    # Add the mimetype file first and set it to be uncompressed
    epub.write(MIMETYPE, compress_type=zipfile.ZIP_STORED)
    
    # For the remaining paths in the EPUB, add all of their files using normal ZIP compression
    for p in os.listdir('.'):
        if os.path.isdir(p):
            for f in os.listdir(p):
                log.debug("Writing file '%s/%s'" % (p, f))
                epub.write(os.path.join(p, f), compress_type=zipfile.ZIP_DEFLATED)
    epub.close()
    shutil.move(epub_name, cwd)
    os.chdir(cwd)
    
    return epub_name

def convert(docbook_file):
    path = convert_docbook(docbook_file)
    find_resources(path)
    create_mimetype(path)
    epub = create_archive(path)

    # Clean up the output directory
    shutil.rmtree(path)

    log.info("Created epub archive as '%s'" % epub)

if __name__ == '__main__':
    '''Convert any DocBook xml files passed in as arguments'''
    for db_file in sys.argv[1:]:
        convert(db_file)
