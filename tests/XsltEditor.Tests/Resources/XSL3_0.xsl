<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes"/>

  <xsl:variable name="book-count" select="count(/catalog/book)"/>

  <xsl:template match="/catalog">
    <html>
      <body>
        <h1>Books (<xsl:value-of select="$book-count"/>)</h1>
        <xsl:for-each select="book">
          <div>
            <p><xsl:value-of select="upper-case(title)"/></p>
            <p>Author: <xsl:value-of select="string-join((author, ' [', format-number(position(), '#'), ']'), '')"/></p>
          </div>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
