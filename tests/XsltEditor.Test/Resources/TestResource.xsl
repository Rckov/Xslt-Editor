<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes" encoding="UTF-8" />

	<xsl:template match="/">
		<html>
			<head>
				<meta charset="UTF-8" />
			</head>
			<body>
				<p>
					<xsl:value-of select="book/title" />
				</p>
				<p>
					Author: <xsl:value-of select="book/author" />
				</p>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>