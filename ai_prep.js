const fs = require('fs');
const path = require('path');

// Function to recursively get all C# files in a directory
function getAllFiles(dirPath, arrayOfFiles = [], baseDir = dirPath) {
    const files = fs.readdirSync(dirPath);
    files.forEach(file => {
        // Skip .DS_Store files and bin/obj directories
        if (file === '.DS_Store' || file === 'bin' || file === 'obj') return;

        const fullPath = path.join(dirPath, file);
        if (fs.statSync(fullPath).isDirectory()) {
            arrayOfFiles = getAllFiles(fullPath, arrayOfFiles, baseDir);
        } else {
            // Only include .cs files and .csproj files
            if (file.endsWith('.cs') || file.endsWith('.csproj')) {
                arrayOfFiles.push({
                    fullPath,
                    relativePath: path.relative(baseDir, fullPath)
                });
            }
        }
    });
    return arrayOfFiles;
}

// Function to determine file type and return appropriate section header
function getSectionHeader(filePath) {
    if (filePath.endsWith('.csproj')) {
        return 'Project File';
    }
    if (filePath.includes('Controllers')) {
        return 'Controller';
    }
    if (filePath.includes('Models')) {
        return 'Model';
    }
    if (filePath.includes('Services')) {
        return 'Service';
    }
    if (filePath.includes('Interfaces')) {
        return 'Interface';
    }
    if (filePath.includes('Tests')) {
        return 'Test';
    }
    return 'Source File';
}

// Main function to merge files
function mergeFiles() {
    const projectDir = "BuckshotPlusPlus";
    const outputFile = 'buckshotplusplus.md';
    let mergedContent = '# BuckshotPlusPlus Project Documentation\n\n';

    try {
        // Get all C# related files recursively
        const allFiles = getAllFiles(projectDir);

        // Sort files by type and path
        allFiles.sort((a, b) => {
            const typeA = getSectionHeader(a.relativePath);
            const typeB = getSectionHeader(b.relativePath);
            if (typeA === typeB) {
                return a.relativePath.localeCompare(b.relativePath);
            }
            return typeA.localeCompare(typeB);
        });

        // Group files by section
        let currentSection = '';

        // Process each file
        allFiles.forEach(({fullPath, relativePath}) => {
            try {
                // Read file content
                const content = fs.readFileSync(fullPath, 'utf8');

                // Get section type
                const section = getSectionHeader(relativePath);

                // Add section header if it's a new section
                if (section !== currentSection) {
                    mergedContent += `\n## ${section}s\n\n`;
                    currentSection = section;
                }

                // Add file header and content
                mergedContent += `### File: ${relativePath}\n\n`;
                mergedContent += '```csharp\n';
                mergedContent += content;
                mergedContent += '\n```\n\n';

                console.log(`Processed: ${relativePath}`);
            } catch (err) {
                console.error(`Error processing file ${relativePath}:`, err);
            }
        });

        // Add summary section
        mergedContent += '\n## Project Summary\n\n';
        const summary = allFiles.reduce((acc, file) => {
            const type = getSectionHeader(file.relativePath);
            acc[type] = (acc[type] || 0) + 1;
            return acc;
        }, {});

        mergedContent += '### File Count by Type\n\n';
        Object.entries(summary).forEach(([type, count]) => {
            mergedContent += `- ${type}s: ${count}\n`;
        });
        mergedContent += `\nTotal files processed: ${allFiles.length}\n`;

        // Write merged content to output file
        fs.writeFileSync(outputFile, mergedContent.trim());
        console.log(`\nMerge complete! Output written to: ${outputFile}`);
        console.log(`Total files processed: ${allFiles.length}`);
    } catch (err) {
        console.error('Error during merge process:', err);
        process.exit(1);
    }
}

// Run the script
mergeFiles();