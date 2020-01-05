#pragma once
#ifndef StoryTableManager_h
#define StoryTableManager_h

#include "StorySingleton.h"

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {

    template<int column>
    class StoryTable;

    class StoryTableManager final :public StorySingleton<StoryTableManager>
    {
    public:
        StoryTableManager() = default;
        virtual ~StoryTableManager() override = default;

        //initialize function, take the place of constructor
        virtual bool Initialize() override;

        //destroy function, take the  place of destructor
        virtual bool Shutdown() override;


        //Getter
        const StoryTable<COLOR_MAX_COLUMN>* const GetColorTable() const;
        StoryTable<COLOR_MAX_COLUMN>*  GetColorTable();

        const StoryTable<FONT_SIZE_MAX_COLUMN>* const GetFontSizeTable() const;
        StoryTable<FONT_SIZE_MAX_COLUMN>*  GetFontSizeTable();

        const StoryTable<TIP_MAX_COLUMN>* const GetTipTable() const;
        StoryTable<TIP_MAX_COLUMN>*  GetTipTable();

        const StoryTable<PAINT_MOVE_MAX_COLUMN>* const GetPaintMoveTable() const;
        StoryTable<PAINT_MOVE_MAX_COLUMN>*  GetPaintMoveTable();

        const StoryTable<CHAPTER_COLUMN>* const GetChapterTable() const;
        StoryTable<CHAPTER_COLUMN>*  GetChapterTable();

        const StoryTable<SCENE_COLUMN>* const GetSceneTable() const;
        StoryTable<SCENE_COLUMN>*  GetSceneTable();

        const StoryTable<CHARACTER_COLUMN>* const GetCharacterTable() const;
        StoryTable<CHARACTER_COLUMN>*  GetCharacterTable();

        const StoryTable<PAUSE_MAX_COLUMN>* const GetPauseTable() const;
        StoryTable<PAUSE_MAX_COLUMN>*  GetPauseTable();
    protected:
    private:


        StoryTable<COLOR_MAX_COLUMN>*  m_colorTable;
        StoryTable<FONT_SIZE_MAX_COLUMN>* m_fontSizeTable;
        StoryTable<TIP_MAX_COLUMN>* m_tipTable;
        StoryTable<PAINT_MOVE_MAX_COLUMN>* m_paintMovetable;
        StoryTable<CHAPTER_COLUMN>* m_chapterTable;
        StoryTable<SCENE_COLUMN>* m_sceneTable;
        StoryTable<CHARACTER_COLUMN>* m_characterTable;
        StoryTable<PAUSE_MAX_COLUMN>* m_pauseTable;


    };


}
#endif
