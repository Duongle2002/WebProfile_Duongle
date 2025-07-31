// Theme Loader - Loads and applies theme settings from database
class ThemeLoader {
    constructor() {
        this.themeSettings = null;
        this.init();
    }

    async init() {
        try {
            // Load theme settings from server
            const response = await fetch('/Admin/GetThemeSettings');
            if (response.ok) {
                this.themeSettings = await response.json();
                this.applyTheme();
            } else {
                console.warn('Could not load theme settings, using defaults');
                this.applyDefaultTheme();
            }
        } catch (error) {
            console.error('Error loading theme settings:', error);
            this.applyDefaultTheme();
        }
    }

    applyTheme() {
        if (!this.themeSettings) {
            this.applyDefaultTheme();
            return;
        }

        const root = document.documentElement;
        
        // Apply color settings
        root.style.setProperty('--primary-color', this.themeSettings.primaryColor || '#007bff');
        root.style.setProperty('--secondary-color', this.themeSettings.secondaryColor || '#6c757d');
        root.style.setProperty('--background-color', this.themeSettings.backgroundColor || '#ffffff');
        root.style.setProperty('--text-color', this.themeSettings.textColor || '#333333');
        
        // Apply button gradient settings
        root.style.setProperty('--primary-button-gradient-start', this.themeSettings.primaryButtonGradientStart || '#007bff');
        root.style.setProperty('--primary-button-gradient-end', this.themeSettings.primaryButtonGradientEnd || '#0056b3');
        root.style.setProperty('--secondary-button-gradient-start', this.themeSettings.secondaryButtonGradientStart || '#6c757d');
        root.style.setProperty('--secondary-button-gradient-end', this.themeSettings.secondaryButtonGradientEnd || '#545b62');
        root.style.setProperty('--outline-button-border-color', this.themeSettings.outlineButtonBorderColor || '#007bff');
        root.style.setProperty('--outline-button-text-color', this.themeSettings.outlineButtonTextColor || '#007bff');
        
        // Apply typography settings
        root.style.setProperty('--font-family', this.themeSettings.fontFamily || "'Inter', sans-serif");
        root.style.setProperty('--font-size', (this.themeSettings.fontSize || 16) + 'px');
        root.style.setProperty('--line-height', this.themeSettings.lineHeight || '1.6');
        root.style.setProperty('--letter-spacing', this.themeSettings.letterSpacing || '0.5px');
        
        // Apply visual settings
        root.style.setProperty('--border-radius', this.themeSettings.borderRadius || '8px');
        root.style.setProperty('--box-shadow', this.themeSettings.boxShadow || '0 2px 4px rgba(0,0,0,0.1)');
        
        // Apply animation settings
        root.style.setProperty('--transition-duration', this.themeSettings.transitionDuration || '0.3s');
        root.style.setProperty('--transition-timing', this.themeSettings.transitionTimingFunction || 'ease');
        
        // Apply gradient if enabled
        if (this.themeSettings.useGradientBackground) {
            const gradient = `linear-gradient(${this.themeSettings.gradientDirection}, ${this.themeSettings.gradientStartColor}, ${this.themeSettings.gradientEndColor})`;
            root.style.setProperty('--background-gradient', gradient);
            root.style.setProperty('--background-color', 'transparent');
        }

        // Load Google Fonts if needed
        this.loadGoogleFonts(this.themeSettings.fontFamily);

        console.log('Theme applied successfully:', this.themeSettings);
    }

    applyDefaultTheme() {
        const root = document.documentElement;
        
        // Default modern theme
        root.style.setProperty('--primary-color', '#3b82f6');
        root.style.setProperty('--secondary-color', '#64748b');
        root.style.setProperty('--background-color', '#ffffff');
        root.style.setProperty('--text-color', '#1e293b');
        root.style.setProperty('--font-family', "'Inter', sans-serif");
        root.style.setProperty('--font-size', '16px');
        root.style.setProperty('--line-height', '1.6');
        root.style.setProperty('--letter-spacing', '0.5px');
        root.style.setProperty('--border-radius', '12px');
        root.style.setProperty('--box-shadow', '0 4px 8px rgba(0,0,0,0.15)');
        root.style.setProperty('--transition-duration', '0.3s');
        root.style.setProperty('--transition-timing', 'ease');

        console.log('Default theme applied');
    }

    loadGoogleFonts(fontFamily) {
        if (!fontFamily || fontFamily.includes('Arial') || fontFamily.includes('Times')) {
            return; // Don't load for system fonts
        }

        const fontName = fontFamily.replace(/['"]/g, '').split(',')[0].trim();
        const link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = `https://fonts.googleapis.com/css2?family=${fontName}:wght@300;400;500;600;700&display=swap`;
        document.head.appendChild(link);
    }

    // Method to update theme dynamically (for admin panel)
    updateTheme(newSettings) {
        this.themeSettings = newSettings;
        this.applyTheme();
    }

    // Get current theme settings
    getCurrentTheme() {
        return this.themeSettings;
    }
}

// Initialize theme loader when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    window.themeLoader = new ThemeLoader();
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ThemeLoader;
} 